using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlayerShoot : NetworkBehaviour
{
    private float shootSpeed = 900, shootTime = 5, ammoCount = 1000, readyForNextShoot = 0, defaultShootTime;
    private float damageMultiplier = 0.75f, defaultDamageMultiplier;
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletSparkle;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerInfo;

    private Coroutine shootTimeCoroutine , damageMultiplierCoroutine; 

    // Start is called before the first frame update
    private void Start()
    {
        defaultDamageMultiplier = damageMultiplier;
        defaultShootTime = shootTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)  return;
        if (Input.GetButton("Fire1"))
        {
            if(Time.time > readyForNextShoot)
            {
                var dir = transform.localRotation.y;
                readyForNextShoot = Time.time + 1 / shootTime;
                RequestFireServerRpc(dir);
            }
        }
    }

    [ServerRpc]
    private void RequestFireServerRpc(float dir)
    {
        RequestFireClientRpc(dir);
    }

    [ClientRpc]
    private void RequestFireClientRpc(float dir)
    {
        Shoot(dir);
    }
    void Shoot(float dir)
    {
        if(ammoCount > 0)
        {
            int direction(float dir)  
            {
                if (dir < 0f)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
                    
            }
            
            ammoCount--;

            GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * direction(dir) * Time.fixedDeltaTime, 0f);
            newBullet.transform.localScale = new Vector2(newBullet.transform.localScale.x * direction(dir), newBullet.transform.localScale.y);
            
            playerMovement.Recoil();


            playerInfo.GetComponent<PlayerMovement>().SetPlayerDamageMultiplier(damageMultiplier) ;

            Debug.Log(GetDamageMultiplier());
            /*velocity.x = thrust * directionRgb();
            rgb.velocity = velocity;*/
            if (bulletSparkle != null)
            {
                Instantiate(bulletSparkle, shootPos.position, Quaternion.identity);
            }

            animator.SetTrigger("isShoot");
            //animator.SetBool("isShoot", false);
        }
        
    }

    IEnumerator FireRate2x()
    {
        float timer = 2f;
        shootTime = defaultShootTime * 2;
        yield return new WaitForSeconds(timer);
        shootTime = defaultShootTime;
        shootTimeCoroutine = null;
    }
    IEnumerator ExtraDamage()
    {
        float timer = 100f;
        damageMultiplier = defaultDamageMultiplier * 50;
        Debug.Log(damageMultiplier);
        yield return new WaitForSeconds(timer);
        damageMultiplier = defaultDamageMultiplier;
        damageMultiplierCoroutine = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("FireRate2x"))
        {
            if(shootTimeCoroutine != null)
            {
                StopCoroutine(shootTimeCoroutine);
            }
            shootTimeCoroutine = StartCoroutine(FireRate2x());
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ExtraDamage"))
        {
            if (damageMultiplierCoroutine != null)
            {
                StopCoroutine(damageMultiplierCoroutine);
            }
            damageMultiplierCoroutine = StartCoroutine(ExtraDamage());
            Destroy(collision.gameObject);
        }
    }
    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }
}


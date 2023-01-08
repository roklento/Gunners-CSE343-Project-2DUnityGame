using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;


public class PlayerShoot : NetworkBehaviour
{
    private float shootSpeed = 900, shootTime = 5, ammoCount = 5, tempAmmoCount, readyForNextShoot = 0, defaultShootTime, reloadtimer = 2f;
    
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletSparkle;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerInfo;

    private Coroutine shootTimeCoroutine;
    private Coroutine ammoCountCoroutine;

    public TextMeshProUGUI ammoMesh;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }
    private void Start()
    {
        ammoMesh = GameObject.Find("Canvas/AMMOCOUNT").GetComponent<TextMeshProUGUI>();
        tempAmmoCount = ammoCount;
        defaultShootTime = shootTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)  return;

        if (ammoCount > -1)
        {
            ammoMesh.text = ammoCount.ToString();
        }
        if (ammoCount == 0)
        {
            if(ammoCountCoroutine != null)
            {
                StopCoroutine(ammoCountCoroutine);
            }
            ammoCountCoroutine = StartCoroutine(Reload());
        }
        
        if (Input.GetButton("Fire1") )
        {
            if (Time.time > readyForNextShoot)
            {
                var dir = transform.localRotation.y;
                readyForNextShoot = Time.time + 1 / shootTime;
                --ammoCount;
                RequestFireServerRpc(dir, ammoCount);
            }

        }

    }

    [ServerRpc]
    private void RequestFireServerRpc(float dir,float ammo)
    {
        RequestFireClientRpc(dir,ammo);
    }

    [ClientRpc]
    private void RequestFireClientRpc(float dir,float ammo)
    {
        Shoot(dir,ammo);
    }
    void Shoot(float dir,float ammo)
    {
        if(ammo >= 0)
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

            GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * direction(dir) * Time.fixedDeltaTime, 0f);
            newBullet.transform.localScale = new Vector2(newBullet.transform.localScale.x * direction(dir), newBullet.transform.localScale.y);
            
            playerMovement.Recoil();


            //playerInfo.GetComponent<PlayerMovement>().SetPlayerDamageMultiplier(damageMultiplier) ;

            //Debug.Log(GetDamageMultiplier());
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
        
    }
    /* float GetDamageMultiplier()
    {
        return damageMultiplier;
    }*/
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadtimer);
        ammoCount = tempAmmoCount;
        ammoCountCoroutine = null;
    }
}


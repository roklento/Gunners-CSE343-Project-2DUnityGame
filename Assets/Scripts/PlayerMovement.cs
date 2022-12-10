using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rgb;
    private Vector2 velocity;
    private bool doubleJump;
    private bool isFalling;
    private float thrustMultiplier = 0.5f;
    private float damageMultiplier = 0.75f, defaultDamageMultiplier;
    private Vector2 thrust;
    private float horizontalInput;
    float speed = 4f, defaultSpeed;
    private Coroutine speedUpCoroutine, damageMultiplierCoroutine;

    //private float playerDamageMultiplier;

    [SerializeField] private LayerMask groundCheckLayers;
    [SerializeField] private float groundCheckRaycastLength;

    private NetworkVariable<bool> isLookingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        defaultDamageMultiplier = damageMultiplier;
        defaultSpeed = speed;
        
        isLookingRight.OnValueChanged += OnIsLookingRightChanged;
        OnIsLookingRightChanged(false, isLookingRight.Value);
    }

    private void OnIsLookingRightChanged(bool oldValue, bool newValue)
    {
        if (!newValue)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        float jumpingSpeed = 6f;

        velocity = rgb.velocity;  

        if(IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            isFalling = false;
        }

        if (Input.GetButtonDown("Jump") && (IsGrounded() || doubleJump))
        {
            velocity.y = jumpingSpeed;
            doubleJump = !doubleJump;
        }

        if (Input.GetButtonDown("Jump") && !isFalling)
        {
            velocity.y = jumpingSpeed;
            isFalling = !isFalling;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        TransformRotation();

        rgb.velocity = velocity;
    }

    private void FixedUpdate()
    {
        //if (!IsOwner) return;
        

        velocity = rgb.velocity;
   
        //speed += Time.fixedDeltaTime;
        velocity.x = horizontalInput * speed;
        //print(velocity.x);
        rgb.velocity = velocity;
        rgb.position = (rgb.position + thrust * Time.deltaTime);
        thrust.x = Mathf.MoveTowards(thrust.x, 0, Time.deltaTime * 2.5f);
        thrust.y = Mathf.MoveTowards(thrust.y, 0, Time.deltaTime * 2.5f);
        

        /*if (Input.GetButton("Fire1"))
        {
            Recoil();
        }*/
        //Mathf.MoveTowards(geri tepme, 0, time)
    }

    void TransformRotation()
    {
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            isLookingRight.Value = false;
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            isLookingRight.Value = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            //currentGameObject = collision.gameObject;
            Damage(collision);
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
        if (collision.gameObject.CompareTag("SpeedUp"))
        {
            if(speedUpCoroutine != null)
            {
                StopCoroutine(speedUpCoroutine);
            }
            speedUpCoroutine = StartCoroutine(SpeedUp());
            Destroy(collision.gameObject);
        }
        
    }
    
    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            //currentGameObject = null;
            isHit = false;
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, new Vector3(0, -groundCheckRaycastLength, 0));
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckRaycastLength, groundCheckLayers);
    }
    int directionRgb()
    {
        if (transform.localRotation.y < 0f)
        {
            //Debug.Log("left");
            return 1;
        }
        else
            return -1;
    }

    public void Recoil()
    {
        //rgb.AddForce(new Vector2(Mathf.MoveTowards(thrust * directionRgb(), 0, Time.deltaTime), 0));
        //velocity.x = Mathf.MoveTowards(thrust * directionRgb(), 0, Time.deltaTime);
        thrust.x += thrustMultiplier * directionRgb();
       //print(velocity.x);
        //rgb.velocity = velocity;
    }

    public void Damage(Collision2D collision)
    {
        int direction;
        //Rigidbody2D newRgb = currentGameObject.GetComponent<Rigidbody2D>();
        //velocity = rgb.velocity;
        Vector2 hitPoint = collision.GetContact(0).point;
        if (transform.position.x > hitPoint.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        thrust.x += damageMultiplier * 2 * direction;
        //velocity.x = Mathf.MoveTowards(thrust * 10, 0, Time.deltaTime);
        //rgb.velocity = velocity;
    }

    public void GrenadeDamage(Vector2 vec)
    {
        float vecLength = Mathf.Pow(vec.magnitude * 5 + 1, 1.5f); //balanced
        vec = vec.normalized;
        thrust += damageMultiplier * vec / vecLength * 147;
        Debug.Log("vecLength " + vecLength);
    }

    IEnumerator SpeedUp()
    {
        float timer = 2f;
        speed = defaultSpeed * 2;
        yield return new WaitForSeconds(timer);
        speed = defaultSpeed;
        speedUpCoroutine = null;
    }

    IEnumerator ExtraDamage()
    {
        float timer = 2f;
        damageMultiplier = defaultDamageMultiplier * 2;
        Debug.Log(damageMultiplier);
        yield return new WaitForSeconds(timer);
        damageMultiplier = defaultDamageMultiplier;
        damageMultiplierCoroutine = null;
    }

    /*private float GetPlayerDamageMultiplier()
    {
        return playerDamageMultiplier;
    }

    public void SetPlayerDamageMultiplier(float PlayerDamageMultiplier)
    {
        playerDamageMultiplier = PlayerDamageMultiplier;
    }*/

}



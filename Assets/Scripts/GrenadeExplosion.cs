using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask playerCheckLayers;
    private Transform explosionPos;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            explosionPos = transform;
            ApplyExplosionForce();
            Destroy(gameObject, 0.005f);
        }
    }
    
    public void OnDrawGizmos()
    {
        if (explosionPos != null)
        {
            Gizmos.DrawWireSphere(explosionPos.position, radius);
        }      
    }

    void ApplyExplosionForce()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(explosionPos.position, radius, playerCheckLayers);
        if (collider != null)
        {
            foreach (Collider2D col in collider)
            {
                /* Debug.Log(col.gameObject.name);
                 Debug.Log(col.transform.position.magnitude);
                 Debug.Log(explosionPos.transform.position.magnitude);*/
                Vector2 vec = col.transform.position - explosionPos.transform.position;
                //vec.Normalize();
                /*vecLength = vec.magnitude;
                Debug.Log("vecLength " + vecLength);

                if (col.transform.position.x < explosionPos.transform.position.x)
                {
                    direction = -1;
                }
                else
                    direction = 1;
                Debug.Log(vecLength * direction);
                col.GetComponent<PlayerMovement>().thrust = vecLength * direction;*/
                col.GetComponent<PlayerMovement>().GrenadeDamage(vec);
            }
        }
    }
}

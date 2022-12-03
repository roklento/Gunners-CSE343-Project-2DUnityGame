using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OneWayPlatform : NetworkBehaviour

{
    private Collider2D currentOneWayPlatform;
    [SerializeField]private BoxCollider2D playerCollider;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            currentOneWayPlatform = collision.collider;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            currentOneWayPlatform = null;
        }
        
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentOneWayPlatform;
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Death : NetworkBehaviour
{
    //private float respawnDelay = 2f;
    //private NetworkVariable<int> lives = new NetworkVariable<int>();
    //private NetworkVariable<Vector2> spawnPoint = new NetworkVariable<Vector2>();
    int lives = 3;
    Vector2 spawnPoint;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }
    void Start()
    {
        lives = 3;
        spawnPoint = transform.position;
        Debug.Log(spawnPoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Here");
            lives--;
            Debug.Log(lives);
            if (lives > 0)
            {
                ClientRespawnClientRpc();
            }
            else
            {
                Destroy(this.gameObject);
                Debug.Log("GameOver" + this);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Here");
            lives--;
            Debug.Log(lives);
            if (lives > 0)
            {
                ClientRespawnClientRpc();
            }
            else
            {
                this.gameObject.SetActive(false);
                Debug.Log("GameOver" + this);
            }
        }
    }
   
    [ClientRpc]
    void ClientRespawnClientRpc()
    {
        Respawn();
    }

    void Respawn()
    {
        transform.position = spawnPoint;
    }
}

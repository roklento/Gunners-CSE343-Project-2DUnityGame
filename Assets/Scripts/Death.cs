using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using System;

public class Death : NetworkBehaviour
{
    public static Death Instance { get; private set; }
    //private float respawnDelay = 2f;
    //private NetworkVariable<int> lives = new NetworkVariable<int>(3 , NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //private NetworkVariable<Vector2> spawnPoint = new NetworkVariable<Vector2>();

    public bool isDead = false;

    public TextMeshProUGUI textMesh;
    int lives = 3;

    Vector2 spawnPoint;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }
    void Start()
    {
        textMesh = GameObject.Find("Canvas/health").GetComponent<TextMeshProUGUI>();
        //lives = 3;
        spawnPoint = transform.position;
        Debug.Log(spawnPoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            /*if (IsClient)
            {
                Debug.Log("Here");
                lives--;
                textMesh.text = lives.ToString();
                ClientRespawnClientRpc();
            }
            else
            {
                Debug.Log("Here");
                lives--;
                textMesh.text = lives.ToString();
            }*/
                Debug.Log("Here");
                lives--;
                textMesh.text = lives.ToString();
                Respawn();

        }
    }

    void Respawn()
    {
        if (lives > 0)
        {
            transform.position = spawnPoint;
        }
        else
        {
            
            if(IsServer)
            {
                isDead = true;
                //Destroy(gameObject);
            }

            else
            {
                if(gameObject != null)
                    DestroyServerRpc();
            }
            
           
        }
    }

    [ServerRpc]
    void DestroyServerRpc()
    {
        isDead = true;
        Destroy(gameObject);
    }

}

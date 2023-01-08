using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameOver : NetworkBehaviour
{
    private NetworkVariable<bool> client0 = new NetworkVariable<bool>();
    private NetworkVariable<bool> client1 = new NetworkVariable<bool>();


    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            Client0();
            //client1.Value = NetworkManager.Singleton.ConnectedClients[].PlayerObject.GetComponent<Death>().isDead;
            //NetworkManager.Singleton.ConnectedClients[1].PlayerObject.GetComponent<Death>()
        }
    }


    void Update()
    {
        if(OwnerClientId == 0)
        {
            
        }

        Debug.Log(OwnerClientId);
        Debug.Log(client0.Value);
        //Debug.Log(OwnerClientId);
        //Debug.Log(client1.Value);
    }

    bool Client0()
    {
        client0.Value = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<Death>().isDead;
        return client0.Value;
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManageUI : MonoBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    public string ipAddress = "127.0.0.1";
    //UnityTransport transport;

    private void Awake()
    {
        serverButton.onClick.AddListener(() =>{
            NetworkManager.Singleton.StartServer();
        });
        HostButton();
        ClientButton();
    }

    public void HostButton()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ipAddress,
            7777,
            "0.0.0.0"
            );
        NetworkManager.Singleton.StartHost();
    }

    public void ClientButton()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ipAddress,
            7777,
            "0.0.0.0"
            );
        NetworkManager.Singleton.StartClient();
 
    }
    public void ChangedIP(string newIPAddress)
    {
        ipAddress = newIPAddress;
    }
}

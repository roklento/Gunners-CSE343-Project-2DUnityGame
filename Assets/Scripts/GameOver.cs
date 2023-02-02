using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameOver : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI winPlayerName;
    [SerializeField] private Canvas wonCanvas;

    //private NetworkVariable<NetworkString> wonPlayer = new NetworkVariable<NetworkString>();

    private bool isGameStart = false;
    private void Awake()
    {
        Hide();
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }

    void Update()
    {
        if(IsServer)
        {
            if (LobbyManager.Instance.isGameStart)
            {
                StartCoroutine(GameStart());

                Debug.Log(isGameStart);
                if(isGameStart)
                {
                    if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject != null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject == null &&
                        NetworkManager.Singleton.ConnectedClientsList[2].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[3].PlayerObject == null)
                    {
                        Show();
                        winPlayerName.text = "Player 0";
                        //NetworkManager.Singleton.
                    }
                    else if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject != null
                        && NetworkManager.Singleton.ConnectedClientsList[2].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[3].PlayerObject == null)
                    {
                        Show();
                        winPlayerName.text = "Player 1";
                    }
                    else if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject == null &&
                        NetworkManager.Singleton.ConnectedClientsList[2].PlayerObject != null && NetworkManager.Singleton.ConnectedClientsList[3].PlayerObject == null)
                    {
                        Show();
                        winPlayerName.text = "Player 2";

                    }
                    else if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject == null &&
                        NetworkManager.Singleton.ConnectedClientsList[2].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[3].PlayerObject != null)
                    {
                        Show();
                        winPlayerName.text = "Player 3";
                    }
                }
            }
        }
    }
    private void Hide()
    {
        wonCanvas.gameObject.SetActive(false);
    }

    private void Show()
    {
        wonCanvas.gameObject.SetActive(true);
    }

    IEnumerator GameStart()
    {
        float timer = 10f;
        yield return new WaitForSeconds(timer);
        isGameStart = true;
    }

}

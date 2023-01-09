using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameOver : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI winPlayerName;
    [SerializeField] private Canvas wonCanvas;

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
                Debug.Log("here");

                StartCoroutine(GameStart());

                Debug.Log(isGameStart);
                if(isGameStart)
                {
                    if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject != null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject == null)
                    {
                        Show();
                        winPlayerName.text = "Player 0";
                        Debug.Log("player 0");
                    }
                    else if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject != null)
                    {
                        Show();
                        winPlayerName.text = "Player 1";
                        Debug.Log("player 1");
                    }
                    /*else if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject == null &&
                        NetworkManager.Singleton.ConnectedClientsList[2].PlayerObject != null && NetworkManager.Singleton.ConnectedClientsList[3].PlayerObject == null)
                    {
                        winPlayerName.text = "Player 2";
                        Debug.Log("win player2");
                    }
                    else if (NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject == null &&
                        NetworkManager.Singleton.ConnectedClientsList[2].PlayerObject == null && NetworkManager.Singleton.ConnectedClientsList[3].PlayerObject != null)
                    {
                        winPlayerName.text = "Player 3";
                        Debug.Log("win player3");
                    }*/
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

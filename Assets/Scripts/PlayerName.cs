using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class PlayerName : NetworkBehaviour
{

    [SerializeField] Transform character;

    public static PlayerName Instance { get; private set; }

    Lobby lobby;
    Player player;

    private void Awake()
    {
        Instance = this;
        player = LobbyManager.Instance.GetPlayer();

    }

    private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();
    private string tempString;
    private bool overlaySet = false;

    public override void OnNetworkSpawn()
    {

        /*if (IsClient)
            Debug.Log("here");
            tempString = EditPlayerName.Instance.GetPlayerName();*/

        //Debug.Log(lobby.Id);

        if (!IsServer)
        {
            /*foreach (Player player in lobby.Players)
            {
                Debug.Log(player.Data[LobbyManager.KEY_PLAYER_NAME].Value);
                if (player != null)
                {
                    if (player.Id == AuthenticationService.Instance.PlayerId)
                    {
                        playerName.Value = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
                    }
                }
                if(LobbyManager.Instance.IsLobbyHost())
                {
                    Debug.Log(player.Data[LobbyManager.KEY_PLAYER_NAME].Value);
                }
                //playerName.Value = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
            }*/
            //tempString = EditPlayerName.Instance.GetPlayerName();
            //playerName.Value = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
            //playerName.Value = $"player {OwnerClientId}";
            return;
        }

        playerName.Value = $"Player {OwnerClientId}";

        

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshPro>();
        localPlayerOverlay.text = playerName.Value;
    }

    private void Update()
    {
        transform.position = character.transform.position + new Vector3(0, 1, 0);
        transform.rotation = Quaternion.identity;

        if (!overlaySet && !string.IsNullOrEmpty(playerName.Value))
        {
            SetOverlay();
            overlaySet = true;
        }
            
    }


}



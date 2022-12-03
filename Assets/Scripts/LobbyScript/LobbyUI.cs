using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button createLobbyButton;

    private void Awake()
    {
        Instance = this;

        lobbySingleTemplate.gameObject.SetActive(false);

        refreshButton.onClick.AddListener(RefreshButtonClick);
        createLobbyButton.onClick.AddListener(CreateLobbyButtonClick);

    }

    private void Start()
    {
        TestLobby.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        TestLobby.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        TestLobby.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        TestLobby.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;

    }

    private void LobbyManager_OnKickedFromLobby(object sender, TestLobby.LobbyEventArgs e)
    {
        //Show();
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        //Show();
    }

    private void LobbyManager_OnJoinedLobby(object sender, TestLobby.LobbyEventArgs e)
    {
        Hide();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, TestLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == lobbySingleTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach(Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
            lobbySingleTransform.gameObject.SetActive(true);
            
        }
    }

    private void RefreshButtonClick()
    {
        TestLobby.Instance.RefreshLobbyList();
    }

    private void CreateLobbyButtonClick()
    {
        TestLobby.Instance.CreateLobby("a", 4, false, TestLobby.GameMode.CaptureTheFlag);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

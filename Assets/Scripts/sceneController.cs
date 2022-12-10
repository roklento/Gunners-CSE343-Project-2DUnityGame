using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UNET;
using UnityEngine;


public class sceneController : MonoBehaviour
{

    public void LoadPlayScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
    public void LoadLobbyScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }
    public void LoadStartScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EnteranceScreen");
    }
    public void LoadSettingsScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OptionMenu");
    }
    public void LoadCampaignScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("campaignScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quiting This Game!");
    }

    


}

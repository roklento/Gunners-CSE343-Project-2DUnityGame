using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMusicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);    
    }
    public void SetAudio(float value)
    {
        AudioListener.volume = value;

    }

    
}

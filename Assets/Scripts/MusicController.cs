using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public void setMusicVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}

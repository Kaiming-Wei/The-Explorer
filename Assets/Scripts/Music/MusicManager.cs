using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManagerInstance;
    public AudioSource audioSrc;
    float musicVolume = 1f;

    void Awake()
    {
        if (musicManagerInstance == null)
        {
            musicManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        musicVolume = PlayerPrefs.GetFloat("Volume value");
        audioSrc.volume = musicVolume;
    }
}

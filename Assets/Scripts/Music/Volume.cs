using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{

    public AudioSource audioSrc;

    Slider VolumeSlider;

    float musicVolume = 1f;


    void Start()
    {
        if(GameObject.Find("Volume_Slider") != null)
        {
            VolumeSlider = GameObject.Find("Volume_Slider").GetComponent<Slider>();
            VolumeSlider.value = musicVolume;
        }
    }

    void Awake()
    {
        musicVolume = PlayerPrefs.GetFloat("Volume value");
        audioSrc.volume = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        audioSrc.volume = musicVolume;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("Volume value", musicVolume);
    }

    public void updateVolume(float volume)
    {
        musicVolume = volume;
    }
}

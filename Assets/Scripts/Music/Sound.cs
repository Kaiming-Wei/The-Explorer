using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    public AudioSource audioSrc;

    Slider SoundSlider;

    float musicSound = 1f;

    void Start()
    {
        if (GameObject.Find("Sound_Slider") != null)
        {
            SoundSlider = GameObject.Find("Sound_Slider").GetComponent<Slider>();
            SoundSlider.value = musicSound;
        }
    }

    void Awake()
    {
        musicSound = PlayerPrefs.GetFloat("Sound value");
        audioSrc.volume = musicSound;
    }

    // Update is called once per frame
    void Update()
    {
        audioSrc.volume = musicSound;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("Sound value", musicSound);
    }

    public void updateSound(float sound)
    {
        musicSound = sound;
    }
}

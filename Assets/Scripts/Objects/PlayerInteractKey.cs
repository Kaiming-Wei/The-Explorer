using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractKey : MonoBehaviour
{

    public GameObject key;
    public GameObject keyImage;

    public new AudioSource audio;
    float musicSound = 0.0f;
    public static bool hasKey = false;

    void Start()
    {
        int i = PlayerPrefs.GetInt("Key", -1);

        if (LoadPlayerPos.isResume == 1 && i == 1)
        {
            hasKey = true;   //Set key to 0 for the new map
            keyImage.SetActive(true);
            key.SetActive(false);
        }
        else
        {
            hasKey = false;   //Set key to 0 for the new map
            keyImage.SetActive(false);
        }

        //Retrieve the sound value from the prefab
        musicSound = PlayerPrefs.GetFloat("Sound value");
        audio.volume = musicSound;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            audio.Play();
            key.SetActive(false); // Hide key gameobject
            hasKey = true;        // Got the key
            keyImage.SetActive(true);
        }
    }
}

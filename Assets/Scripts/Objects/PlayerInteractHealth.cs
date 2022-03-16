using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractHealth : MonoBehaviour
{
    private const int healthCount = 4;

    public GameObject[] backHealthObject = new GameObject[healthCount];
    public GameObject[] foreHealthObject = new GameObject[healthCount];
    public static int currentHealth = 4;
    public static int isHealthIcon = 0;
    public GameObject healthIcon;

    public AudioSource healthPickup;
    float musicSound = 0.0f;

    void Start()
    {
        // If is new game, ignore othewise retrieve hp
        if(LoadPlayerPos.isResume == 1)
        {
            currentHealth = PlayerPrefs.GetInt("Player_Hp_count", 0);

            if(PlayerPrefs.GetInt("Player_Hp_Icon", -1) == 1)
                healthIcon.SetActive(false);
        }

        // Retrieve hp
        for (int i = 0; i < currentHealth; ++i)
            foreHealthObject[i].SetActive(true);

        //Retrieve the sound value from the prefab
        musicSound = PlayerPrefs.GetFloat("Sound value");
        healthPickup.volume = musicSound;
    }

    void Update()
    {
        // Decrease health count when the player take damage
        if (currentHealth < healthCount && currentHealth >= 0)
        {
            foreHealthObject[currentHealth].SetActive(false);
        }
        else if(currentHealth < 0)
        {
            foreHealthObject[0].SetActive(false);
        }
    }

    // Player interact with health icon
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Health"))
        {
            if(currentHealth != healthCount)
            {
                healthPickup.Play();
                healthIcon.SetActive(false);
                isHealthIcon = 1;
                foreHealthObject[currentHealth].SetActive(true);
                currentHealth++;
            }
        }
    }
}
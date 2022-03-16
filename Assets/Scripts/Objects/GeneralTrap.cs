using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTrap : MonoBehaviour
{
    Damage damage;

    public AudioClip hurtSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("Sound value");

        damage = transform.GetComponent<Damage>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(hurtSound);
        }

        damage.onDamage(other.gameObject);
    }
}

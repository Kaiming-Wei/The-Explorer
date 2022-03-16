using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWater : MonoBehaviour
{
    Damage damage;
    public AudioClip poolJumpSound;
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
            audioSource.PlayOneShot(poolJumpSound);
            audioSource.PlayOneShot(hurtSound);
        }

        damage.onDamage(other.gameObject);
    }
}

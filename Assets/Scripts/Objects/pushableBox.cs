using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableBox : MonoBehaviour
{
    public SpriteRenderer boxRenderer;
    public Sprite boxSprite;
    public AudioSource sound;
    private bool isActivate = false;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivate)
        {
            boxRenderer.sprite = boxSprite;
            isActivate = true;
        }
        sound.Play();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        sound.Stop();
    }
}

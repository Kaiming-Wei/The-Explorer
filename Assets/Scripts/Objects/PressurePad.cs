using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer spriteRenderer;
    public Sprite oldSprite;
    public Sprite newSprite;
    public float jumpForce;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Set jump force value
            Player_Character.jumpForce = jumpForce;

            //Switch sprite
            spriteRenderer.sprite = newSprite;

            // Switch back the old sprite
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.sprite = oldSprite;
    }
}

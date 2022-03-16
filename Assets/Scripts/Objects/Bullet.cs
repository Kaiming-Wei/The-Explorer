using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;
    public float speed;
    Damage damage;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();
        animator = transform.GetComponent<Animator>();
        // SetDirection(false);
    }

    public void SetDirection(bool goRight) {
        spriteRenderer.flipX = !goRight;
        rigidbody2d.velocity = new Vector2(goRight ? speed : -speed, 0);
    } 

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            damage.onDamage(collision.gameObject);
            rigidbody2d.velocity = Vector2.zero;
            transform.GetComponent<BoxCollider2D>().enabled = false;
            animator.SetBool("bulletFlag", true);
            Destroy(gameObject, 0.15f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

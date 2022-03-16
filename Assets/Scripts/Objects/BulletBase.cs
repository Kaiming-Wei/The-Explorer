using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    protected Rigidbody2D rigidbody2d;
    protected SpriteRenderer spriteRenderer;
    protected Damage damage;
    protected Animator animator;

    public virtual void Awake(){
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();
        animator = transform.GetComponent<Animator>();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision){
        damage.onDamage(collision.gameObject);

        if(animator != null){
            animator.SetBool("isBomb", true);
        }
        
        rigidbody2d.velocity = Vector2.zero;
        transform.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.15f);
    }
}
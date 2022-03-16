using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerProjectile2 : BulletBase
{
    // Start is called before the first frame update
    public Collider2D bullet;

    void Start()
    {
       bullet = GetComponent<Collider2D>();
       bullet.isTrigger = true;
       // Destroy(gameObject, 5);
    }

    public void SetSpeed(Vector2 vector2){
        rigidbody2d.velocity = vector2;
    }

    public void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Bullet"){
            Destroy(collision.gameObject);
            return;
        }
        else if(collision.gameObject.name == "Player"){
            damage.onDamage(collision.gameObject);
            rigidbody2d.velocity = Vector2.zero;
            transform.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 0.15f);
        }
        else{
            bullet.isTrigger = false;
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.name != "Player"){
            return;
        }
        base.OnCollisionEnter2D(collision);
    }

}

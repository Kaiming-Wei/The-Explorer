using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : BulletBase
{
        public float speed = 10f;

        public override void Awake(){
            base.Awake();
            Destroy(gameObject, 5f);
        }

        public void SetDirection(Vector3 direction){
            rigidbody2d.velocity = direction * speed;
        }

        public override void OnCollisionEnter2D(Collision2D collision){
            if(collision.gameObject.name == "Player"){
                damage.onDamage(collision.gameObject);
        
                rigidbody2d.velocity = Vector2.zero;
                transform.GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 0.15f);
            }
        }
}

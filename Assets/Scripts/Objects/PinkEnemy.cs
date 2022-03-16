using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus {
    Idle,
    Walk,
    Attack,
    Run,
    Dead
}

public class PinkEnemy : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float speed;
    public float runningSpeed;
    Transform validPos;
    public bool flipFlag = false;
    public bool flipFlag2 = false;
    public EnemyStatus enemyStatus;
    float idleTimer;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Damage damage;
    DamageController dc;

    public GameObject player;

    private bool invincible = true;

    public AudioClip attackSound;
    public AudioClip deadSound;
    private AudioSource audioSource;

    public float attackRange;
    public float listenRange;

    public Transform attackTarget;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        validPos = transform.Find("validPos");
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        enemyStatus = EnemyStatus.Idle;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        damage = transform.GetComponent<Damage>();
        dc = transform.GetComponent<DamageController>();
        dc.Death += Death;
        attackTarget = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLisenter();
        updateStatus();
        checkValidPos();
    }

    void SetSpeed(float sp) {
        spriteRenderer.flipX = sp < 0;
        rigidbody2d.velocity = new Vector2(sp, rigidbody2d.velocity.y);

    }

    public void checkValidPos() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(validPos.position, Vector2.down, 1, 1<<8);
        RaycastHit2D r2 = Physics2D.Raycast(validPos.position, Vector2.right, 0.2f, 1<<8);
        RaycastHit2D r3 = Physics2D.Raycast(validPos.position, Vector2.left, 0.2f, 1<<8);

        //Distance between chomper and the player
        float dist = Vector3.Distance(transform.position, attackTarget.position);

        if(!r2 && !r3 && attackTarget.position.x <= 30.55f && attackTarget.position.x >= 8.7f && transform.position.y >= 44.0f && attackTarget.position.y >= 44.0f)
        {
            enemyStatus = EnemyStatus.Run;
        }
        else if(!r2 && !r3 && attackTarget.position.x <= 48.0f && attackTarget.position.x >= 22.0f 
            && transform.position.y >= 28.0f && transform.position.y <= 34.77f &&
            attackTarget.position.y >= 28.0f && attackTarget.position.y <= 34.77f)
        {
            enemyStatus = EnemyStatus.Run;
        }
        else if (!r2 && !r3 && attackTarget.position.x <= 34.0f && attackTarget.position.x >= 17.6f
            && transform.position.y >= 22.8f && transform.position.y <= 27.0f &&
            attackTarget.position.y >= 22.8f && attackTarget.position.y <= 27.0f)
        {
            enemyStatus = EnemyStatus.Run;
        }
        else if (!r2 && !r3 && attackTarget.position.x <= 41.67f && attackTarget.position.x >= 30.59f
            && transform.position.y >= 4.73f && transform.position.y <= 8.7f &&
            attackTarget.position.y >= 4.73f && attackTarget.position.y <= 8.7f)
        {
            enemyStatus = EnemyStatus.Run;
        }
        else if (!r2 && !r3 && attackTarget.position.x <= 68.05f && attackTarget.position.x >= 60.23f
            && transform.position.y >= 14.05f && transform.position.y <= 18.02f &&
            attackTarget.position.y >= 14.05f && attackTarget.position.y <= 18.02f)
        {
            enemyStatus = EnemyStatus.Run;
        }
        else
        {
            enemyStatus = EnemyStatus.Walk;
        }

        if (r2 || r3) {

            flipFlag = true;
            enemyStatus = EnemyStatus.Walk;

            return;
        }
        flipFlag = raycastHit2D;
        flipFlag = !flipFlag;

    }

    public void updateStatus() {
        switch (enemyStatus)
        {
            case EnemyStatus.Idle:
                // SetSpeed(0);
                idleTimer += Time.deltaTime;
                if (idleTimer > 1) {
                    idleTimer = 0;
                    enemyStatus = EnemyStatus.Walk;
                }
                break;
            case EnemyStatus.Walk:
                if (flipFlag) {
                    speed = -speed;
                    validPos.localPosition = new Vector3(-validPos.localPosition.x, validPos.localPosition.y, validPos.localPosition.z);
                }
                SetSpeed(speed);
                animator.SetBool("isWalk", true);
                break;
            case EnemyStatus.Dead:
                animator.SetBool("isDead", true);
                break;
            case EnemyStatus.Run:
                if((speed < 0 && transform.position.x < attackTarget.position.x) || (speed > 0 && transform.position.x > attackTarget.position.x))
                {
                    flipFlag = true;
                }

                if (flipFlag)
                {
                    speed = -speed;
                    validPos.localPosition = new Vector3(-validPos.localPosition.x, validPos.localPosition.y, validPos.localPosition.z);
                }
                Debug.Log(flipFlag);

                SetSpeed(speed*2);
                animator.SetBool("isRun", true);

                break;  
            case EnemyStatus.Attack:
                animator.SetBool("attack", true);
                break;
            default:
                break;
        }

        if (enemyStatus != EnemyStatus.Walk) {
            animator.SetBool("isWalk", false);
        }
        if (enemyStatus != EnemyStatus.Run) {
            animator.SetBool("isRun", false);
        }
        // if (enemyStatus != EnemyStatus.Attack) {
        //     animator.SetBool("attack", false);
        // }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {

            if(PlayerInteractHealth.currentHealth > 1)
                player.GetComponent<Collider2D>().enabled = false;

            audioSource.PlayOneShot(attackSound, 1.0f);
            damage.onDamage(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(4.0f);
        invincible = true;
    }

    public void Death() {
        audioSource.PlayOneShot(deadSound, 1.0f);
        SetSpeed(0);
        enemyStatus = EnemyStatus.Dead;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rigidbody2d.gravityScale = 0;
        rigidbody2d.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmos() {
        // UnityEditor.Handles.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.2f);
        // UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, attackRange);
        // UnityEditor.Handles.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
        // UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, listenRange);
    }
    public void UpdateLisenter() {
        if (attackTarget == null) {
            return;
        }

        // if (Vector3.Distance(transform.position, attackTarget.position) <= attackRange) {
        //     enemyStatus = EnemyStatus.Attack;
        //     return;
        // } 
        // else {
        //     enemyStatus = EnemyStatus.Idle;
        // }

        // if (Vector3.Distance(transform.position, attackTarget.position) <= listenRange) {
        //     enemyStatus = EnemyStatus.Run;
        // } 
        // else {
        //     enemyStatus = EnemyStatus.Walk;
        // }
    }
}

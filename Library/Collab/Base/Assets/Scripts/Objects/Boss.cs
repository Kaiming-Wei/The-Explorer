using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum BossStatus{
    Idle,
    Attack,
    Disable,
    Dead
}

public class Boss : MonoBehaviour
{
    Animator animator;

    DamageController bossAble, defenceAble;

    public BossStatus currentStatus = BossStatus.Idle;
    int defaultDenfenceHP;
    int defaultBossHP;


    public float attackTime = 3;

    public GameObject bossBullet1;
    public Transform Bullet1Pos;

    public GameObject bossBullet2;
    public Transform Bullet2Pos;

    public BulletLighting bossBullet3;
    GameObject attackTarget;

    public BossPanel bossPanel;

    public GameObject wonPanel;

    public AudioClip bulletSound;
    public AudioClip bullet2Sound;
    public AudioClip lighteningSound;
    public AudioClip DeadSound;
    public AudioClip ShieldActivateSound;
    public AudioClip ShieldDeactivateSound;
    public AudioSource shieldSoundLoop;

    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        bossAble = transform.GetComponent<DamageController>();
        bossAble.Hurt += this.OnBossHurt;
        bossAble.Death += this.OnBossDead; 

        defenceAble = transform.Find("BossShield").GetComponent<DamageController>();
        defenceAble.Hurt += this.OnDefenceHurt;
        defenceAble.Death += this.OnDefenceDead; 

        defaultDenfenceHP = defenceAble.health;
        defaultBossHP = bossAble.health;

        attackTarget = GameObject.Find("Player");
        StartAttack();

        shieldSoundLoop.Play();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateStatus();
    }

    public void OnUpdateStatus(){
        switch (currentStatus){
            case BossStatus.Idle:
                break;
            case BossStatus.Attack:
                break;
            case BossStatus.Disable:
                animator.SetBool("isDisable", true);
                break;
            case BossStatus.Dead:
                animator.SetBool("isDead", true);
                break;
        }

        if(currentStatus != BossStatus.Disable){
            animator.SetBool("isDisable", false);
        }
    }

    public void OnBossHurt(){
        if (defenceAble.health > 0)
        {
            bossAble.health++;
            defenceAble.takeDamage(1);
            return;
        }

        // update Boss health
        bossPanel.UpdateBossHP((float)bossAble.health / (float)defaultBossHP);
    }

    public void OnBossDead(){
        if (defenceAble.health > 0)
        {
            bossAble.health++;
            defenceAble.takeDamage(1);
            return;
        }

        audioSource.PlayOneShot(DeadSound, 0.5f);
        currentStatus = BossStatus.Dead;
        animator.SetTrigger("Trigger");
        CancelInvoke("ResetDenfence");
        // update Boss health
        bossPanel.UpdateBossHP((float)bossAble.health / (float)defaultBossHP);

        transform.GetComponent<Rigidbody2D>().gravityScale = 0;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        //Show the won panel after 5 seconds
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5.0f);

        wonPanel.SetActive(true);
    }

    public void OnDefenceHurt(){
        // update shield health
        bossPanel.UpdateDefenceHP((float)defenceAble.health / (float)defaultDenfenceHP);
    }

    public void OnDefenceDead(){

        audioSource.PlayOneShot(ShieldDeactivateSound, 0.5f);
        shieldSoundLoop.Stop();

        // update shield health
        bossPanel.UpdateDefenceHP((float)defenceAble.health / (float)defaultDenfenceHP);
        bossPanel.ChangeSprite();


        currentStatus = BossStatus.Disable;
        animator.SetTrigger("Trigger");
        Invoke("ResetDenfence", 5);

        StopAttack();
    }

    public void ResetDenfence(){
        audioSource.PlayOneShot(ShieldActivateSound, 0.5f);
        shieldSoundLoop.Play();

        currentStatus = BossStatus.Idle;
        defenceAble.health = defaultDenfenceHP;

        // update shield health
        bossPanel.ChangeSprite();
        bossPanel.UpdateDefenceHP((float)defenceAble.health / (float)defaultDenfenceHP);

        StartAttack();
    }

    // Boss get into attack mode
    public void StartAttack(){
        InvokeRepeating("Attack", attackTime, attackTime);
    }

    public void Attack(){
        int attackType = Random.Range(1,4);
        //int attackType = 1;
        animator.SetFloat("AttackType", attackType);
        animator.SetTrigger("Attack");
    }

    // Quit attack mode
    public void StopAttack(){
        CancelInvoke("Attack");
    }

    public void AttackExc(int type){
        switch(type){
            case 1:
                audioSource.PlayOneShot(bullet2Sound, 0.5f);
                AttackType1();
                break;
            case 2:
                audioSource.PlayOneShot(bulletSound, 0.5f);
                AttackType2();
                break;
            case 3:
                audioSource.PlayOneShot(lighteningSound, 0.5f);
                AttackType3();
                break;
        }
    }

    public void AttackType1(){
        GameObject bulletObj1 = GameObject.Instantiate(bossBullet1);
        bulletObj1.transform.position = Bullet1Pos.position;
        bulletObj1.GetComponent<BossProjectile>().SetDirection(((attackTarget.transform.position + Vector3.up) - Bullet1Pos.position).normalized);
    }

    public void AttackType2(){

        GameObject bullet2 = GameObject.Instantiate(bossBullet2);
        bullet2.transform.position = Bullet2Pos.position;
        float g = Mathf.Abs(Physics2D.gravity.y)*bullet2.transform.GetComponent<Rigidbody2D>().gravityScale;
        float v0 = 3.7f;
        float t0 = v0 / g;
        float y0 = 0.5f * g * t0 * t0;
        float v = 0;
        float x = attackTarget.transform.position.x - transform.position.x + Random.Range(-1.5f, 1.5f);

        if(transform.position.y + y0 > attackTarget.transform.position.y){
            float y = transform.position.y - attackTarget.transform.position.y + y0;
            float t = Mathf.Sqrt((y * 2) / g)+ t0;
            v=x / t;
        }
        else if(transform.position.y + y0 < attackTarget.transform.position.y){
            float y = attackTarget.transform.position.y - transform.position.y;
            float t = Mathf.Sqrt((y*2)/g);
            v0 = g * t;
            v= x / t;
        }
        bullet2.GetComponent<GunnerProjectile2>().SetSpeed(new Vector2(v, v0));
    }

    public void AttackType3(){
        bossBullet3.Show();
    }
}

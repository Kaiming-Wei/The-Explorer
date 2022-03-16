using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum PlayerStatus {
    Idle = 0,
    Jump = 1,
    Run = 2,
    Crouch = 3
}

public enum AttackType {
    Attack = 0,
    Shoot = 1
}

public class Player_Character : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    Animator animator;

    public float speed = 1; // speed on x
    public float speedY;    // speed on y
    private float timerY;
    public static float jumpForce;

    public bool isGround;
    public bool jump = false;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip pressureSound;
    public AudioClip meleeSound;
    public AudioClip bulletSound;
    public AudioSource footAudioSource;
    AudioSource audioSource;

    public PlayerStatus currentStatus = PlayerStatus.Idle;

    Transform followTarget;
    public Vector3 followTargetOffset;

    DamageController DC;
    Damage attackDamage;
    float attackTime = 0.2f;
    bool readyToAttack = true;

    AttackRange attackRange;
    public GameObject range;
    Transform bulletSpawnPos;
    GameObject bulletPrefab;

    BossDoor bossdoor;

    public bool DefaultWeapon;

    //private bool pauseAnim;


    //Respawns positions after hurt
    Dictionary<string, List<Vector3>> respawnPos = new Dictionary<string, List<Vector3>>();

    void Awake()
    {
        // Load respawn positions for every level scenes
        // Loading level 1 respawn positions
        List<Vector3> l1 = new List<Vector3>();
        l1.Add(new Vector3(10, 2, 0));
        respawnPos.Add("Level-01", l1);

        // Loading level 2 respawn positions
        List<Vector3> l2 = new List<Vector3>();
        l2.Add(new Vector3(12, 5, 0));
        l2.Add(new Vector3(28, 9, 0));
        l2.Add(new Vector3(52, 1.5f, 0));
        l2.Add(new Vector3(75, 5, 0));
        respawnPos.Add("Level-02", l2);

        // Loading level 3 respawn positions
        List<Vector3> l3 = new List<Vector3>();
        l3.Add(new Vector3(31.44f, 46.59f, 0));
        l3.Add(new Vector3(49.21f, 45.76f, 0));
        l3.Add(new Vector3(12.48f, 28.77f, 0));
        l3.Add(new Vector3(68.26f, 23.56f, 0));
        l3.Add(new Vector3(55.62f, 14.7f, 0));
        l3.Add(new Vector3(8.26f, 15.84f, 0));
        l3.Add(new Vector3(39.3f, 23.6f, 0));
        respawnPos.Add("Level-03", l3);
    }

    // Start is called before the first frame update
    void Start()
    {   
        if(GameObject.Find("Boss_Spawner")){
            bossdoor = GameObject.Find("Boss_Spawner").GetComponent<BossDoor>();
        }
        audioSource = GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        followTarget = transform.Find("followTarget");
        followTarget.position = transform.position + followTargetOffset;

        footAudioSource.volume = PlayerPrefs.GetFloat("Sound value");
        audioSource.volume = PlayerPrefs.GetFloat("Sound value");

        attackDamage = transform.GetComponent<Damage>();
        DC = transform.GetComponent<DamageController>();
        DC.Hurt += this.Hurt;
        DC.Death += this.Death;

        attackRange = transform.Find("attackRange").GetComponent<AttackRange>();
        range.SetActive(false);

        bulletSpawnPos = transform.Find("bulletSpawnPos");

        Time.timeScale = 1f;
        //pauseAnim = false;

        if(DefaultWeapon){
            Data<bool,bool> data = new Data<bool,bool>();
            data.value1 = true;
            DataManager.Instance.SaveData("have_weapon", data);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        UpdateVelocity();
        CheckGround();
        UpdateStatus();
        UpdateAnimator();
        UpdateFollowTargetPos();
    }

    public void CheckGround() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position + Vector3.up, Vector3.down, 1.5f, 1 << 8);
        isGround = raycastHit2D;
    }

    public void UpdateAnimator() {
        animator.SetBool("isJump", !isGround);
        animator.SetBool("isCrouch", Player_Inputs.instance.Vertival.value == -1);
    }

    public void UpdateStatus() {
        currentStatus = PlayerStatus.Idle;

        if (rigidbody2d.velocity.x != 0) {
            currentStatus = PlayerStatus.Run;
        }
        if (!isGround) {
            currentStatus = PlayerStatus.Jump;
        }
        if (Player_Inputs.instance.Vertival.value == -1 && isGround) {
            currentStatus = PlayerStatus.Crouch;
        }

        if (SceneManager.GetActiveScene().name != "Level-01")
        {
            if (gotWeapon())
            {
                if ((Player_Inputs.instance.Attack.Down || Player_Inputs.instance.Attack.Hold) && isGround && Player_Inputs.enabled)
                {
                    Attack(AttackType.Attack);
                }
                if (Player_Inputs.enabled && (Player_Inputs.instance.Shoot.Down || Player_Inputs.instance.Shoot.Hold))
                {
                    Attack(AttackType.Shoot);
                }
            }
        }
    }

    public void UpdateVelocity() {
        // update speed on x-axis
        if(Pause_Menu.IsInputEnabled)
            SetSpeedX(Player_Inputs.instance.Horizontal.value * speed);
        
        // update speed on u-axis
        if(SetJump()) {
            SetSpeedY(speedY);
        }
    }

    public bool SetJump() {
        if (isGround && Input.GetKeyDown(KeyCode.Space) && currentStatus != PlayerStatus.Crouch)
            audioSource.PlayOneShot(jumpSound);

        if (isGround && jump && currentStatus != PlayerStatus.Crouch)
            audioSource.PlayOneShot(landSound);

        if (Player_Inputs.instance.Jump.Down && isGround) {
            timerY = 0;
            jump = true;
        }
        if (Player_Inputs.instance.Jump.Hold && jump)
        {
            timerY += Time.deltaTime;
            jump = timerY < 0.2f ? true : false;
        }
        if (Player_Inputs.instance.Jump.Up)
        {
            jump = false;
        }
        return jump;
    }

    public void SetSpeedX(float x){

        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && isGround)
        {
            footAudioSource.enabled = true;
            footAudioSource.loop = true;
        }
        else
        {
            footAudioSource.enabled = false;
            footAudioSource.loop = false;
        }

        animator.SetBool("isRun", x!=0);

        if(x > 0){
            spriteRenderer.flipX = false;
            if (SceneManager.GetActiveScene().name != "Level-01") {
                attackRange.transform.localPosition = new Vector3(1.1f, attackRange.transform.localPosition.y, 0);
            }
        }else if(x < 0){
            spriteRenderer.flipX = true;
            if (SceneManager.GetActiveScene().name != "Level-01") {
                attackRange.transform.localPosition = new Vector3(-1.1f, attackRange.transform.localPosition.y, 0);
            }
        }

        if (currentStatus == PlayerStatus.Crouch) {
            x = 0;
        }
        rigidbody2d.velocity = new Vector2(x, rigidbody2d.velocity.y);
    }

    public void SetSpeedY(float y){
        if (currentStatus == PlayerStatus.Crouch) {
            y = 0;
        }
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, y);
    }

    public void UpdateFollowTargetPos() {
        if (spriteRenderer.flipX) {
            followTarget.position = Vector3.MoveTowards(followTarget.position, transform.position - followTargetOffset, 0.1f);
        } else {
            followTarget.position = Vector3.MoveTowards(followTarget.position, transform.position + followTargetOffset, 0.1f);
        }
    }

    public void Hurt()
    {
        if (SceneManager.GetActiveScene().name != "Level-Boss")
        {
            //Get the current scene
            Scene m_Scene = SceneManager.GetActiveScene();

            // Get the player position
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

            List<Vector3> l1 = respawnPos[m_Scene.name];

            animator.SetTrigger("Hurt");
            StartCoroutine(Delay(0.7, closestPos(l1, playerPos)));

            GetComponent<Renderer>().material.color = new Color(1,1,1,1);
        }
        else
        {
            Player_Inputs.enabled = true;
        }
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        //Show death scene after animation
        StartCoroutine(Delay(1.0, new Vector3(0,0,0)));
    }

    IEnumerator Delay(double time, Vector3 pos)
    {
        yield return new WaitForSeconds((float)time);

        if (PlayerInteractHealth.currentHealth <= 0)
        {
            DeathMenu.isDeathPanelActive = true;
        }
        else
        {
            this.GetComponent<Collider2D>().enabled = true;
            animator.SetTrigger("invincible");

            player.transform.position = pos;
            Player_Inputs.enabled = true;
        }
    }

    //Find the closest respawn position
    public Vector3 closestPos(List<Vector3> src, Vector3 target)
    {
        double cx = 10000.0;
        int index = 0;

        if(SceneManager.GetActiveScene().name == "Level-03")
        {
            if (target.x > 7.8 && target.y > 43)
                return src[1];
            else if (target.x > 33 && target.y > 45)
                return src[0];
            else if (target.x > 11.89 && target.y > 28.18)
                return src[2];
            else if (target.x < 39.77 && target.y > 22.99)
                return src[6];
            else if (target.x < 68.56 && target.y > 22.99)
                return src[3];
            else if (target.x > 8.43 && target.y < 16.06)
                return src[5];
            else
                return src[4];
        }
        else
        {
            for(int i = 0; i < src.Count; ++i)
            {
                double distance = Math.Abs(target.x - src[i].x) + Math.Abs(target.y - src[i].y);
                if (distance < cx)
                {
                    cx = distance;
                    index = i;
                }
            }
        }

        return src[index];
    }

    // When the player collide with jumping pad
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpingPlatform"))
        {
            audioSource.PlayOneShot(pressureSound);
            // update speed on u-axis
            SetSpeedY(speedY*jumpForce);
        }
        else if (other.CompareTag("Box"))
        {
            animator.SetFloat("shoot", 2);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            animator.SetFloat("shoot", 1);
        }
    }

    public void Attack(AttackType at) {
        if (!gotWeapon()) return;
        if (!readyToAttack) return;

        Debug.Log("Attacking: " + at);

        animator.SetTrigger("attack");

        animator.SetInteger("attackType", (int)at);

        if ((int)at == 1) {
            audioSource.PlayOneShot(bulletSound, 0.5f);
            Invoke("CreatBullet", 0.04f);
        }
        else
        {
            audioSource.PlayOneShot(meleeSound, 0.5f);
            range.SetActive(true);
        }

        AttackDamage();
        readyToAttack = false;
        Invoke("ResetAttack", attackTime);
    }

    public void ResetAttack() {
        range.SetActive(false);
        readyToAttack = true;
    }

    // Check whether Player got the weapon
    public bool gotWeapon() {
        Data data = DataManager.Instance.GetData(WeaponPickup.have_weapon);
        if (data != null && ((Data<bool,bool>)data).value1) {
            return true;
        }
        return false;
    }

    public void AttackDamage() {
        GameObject[] dcs = attackRange.GetDamageableObjects();
        if (dcs != null && dcs.Length != 0) {
            for (int i = 0; i<dcs.Length; i++) {
                attackDamage.onDamage(dcs[i]);
            }
        }
    }

    void CreatBullet() {
        if (bulletPrefab == null) {
            bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        }
        GameObject gameObject = GameObject.Instantiate(bulletPrefab);
        if ( spriteRenderer.flipX) {
            bulletSpawnPos.localPosition = new Vector3(-bulletSpawnPos.localPosition.x, bulletSpawnPos.localPosition.y, bulletSpawnPos.localPosition.z);
        }
        gameObject.transform.position = bulletSpawnPos.position;
        gameObject.GetComponent<Bullet>().SetDirection(!spriteRenderer.flipX);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.name == "Switch"){
            Destroy(col.gameObject);
            bossdoor.OpenDoor();
        }
    }
}

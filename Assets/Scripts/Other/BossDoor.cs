using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{   
    Animator door, boss;
    public GameObject bossobj;
    public BossPanel bossPanel;
    public AudioClip bossIntr;
    private AudioSource audioSource;

    void Update()
    {
        if (DeathMenu.isDeathPanelActive)
            bossobj.SetActive(false);
    }

    private void Start(){
        door = transform.GetComponent<Animator>();
        boss = transform.Find("Boss").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Play animations
    public void OpenDoor(){
        door.Play("Boss_Door");
        boss.Play("Boss_Spawn");

        Player_Inputs.enabled = false;
        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(transform);
        audioSource.PlayOneShot(bossIntr, 0.5f);
    }

    public void OnOpenDoorOver(){
        // showup the boss
        gameObject.SetActive(false);
        boss.gameObject.SetActive(false);
        bossobj.SetActive(true);
        bossPanel.Show();


        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(GameObject.Find("Player").transform);
        Player_Inputs.enabled = true;
    }

}

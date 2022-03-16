using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite holdWeapon, noWeapon;
    public AudioClip weaponPickup;
    public GameObject notifAnim;
    private AudioSource audioSource;
    private bool validTrigger;

    public const string have_weapon = "have_weapon";

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        Data<bool,bool> data = (Data<bool,bool>) DataManager.Instance.GetData(have_weapon);
        if (data != null && data.value1) {
            spriteRenderer.sprite = noWeapon;
            notifAnim.SetActive(false);
            validTrigger = false;
        }
        else {
            spriteRenderer.sprite = holdWeapon;
            notifAnim.SetActive(true);
            validTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && validTrigger) {
            notifAnim.SetActive(false);
            spriteRenderer.sprite = noWeapon;
            audioSource.PlayOneShot(weaponPickup, 0.5f);
            // Save data
            Data<bool,bool> data = new Data<bool,bool>();
            data.value1 = true;
            DataManager.Instance.SaveData(have_weapon, data);
            TipMessage._instance.showTip("Congratulation! You have a weapon, press K to attack or O to shoot.", TipStyle.Bottom);
            Invoke("HideTip", 2);
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void HideTip() {
        TipMessage._instance.hideTip(TipStyle.Bottom);
    }
}

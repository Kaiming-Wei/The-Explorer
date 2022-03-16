using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPanel : MonoBehaviour
{
    public Slider slider_boss, slider_defence;
    public Image image;
    public Sprite fullShield, brokenShield;


    void Start(){
        image = GameObject.Find("Logo").GetComponent<Image>();
        image.sprite = fullShield;

        slider_boss.enabled = false;
        slider_defence.enabled = false;
    }

    public void UpdateBossHP(float hp){
        slider_boss.value = hp;
    }

    public void UpdateDefenceHP(float hp){
        slider_defence.value = hp;
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void ChangeSprite(){
        if(image.sprite == fullShield){
            image.sprite = brokenShield;
        }
        else{
            image.sprite = fullShield;
        }
    }
}

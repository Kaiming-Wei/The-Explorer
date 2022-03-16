using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InforSign : MonoBehaviour
{   
    public string TipContent= "";

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            TipMessage._instance.showTip(TipContent, TipStyle.Bottom);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.tag == "Player"){
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);
        TipMessage._instance.hideTip(TipStyle.Bottom);
    }
}

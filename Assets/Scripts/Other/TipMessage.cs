using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TipStyle{
    Bottom, 
}

public class TipMessage : MonoBehaviour
{
    GameObject tipObj;

    public static TipMessage _instance;

    private void Awake(){
        _instance = this;
    }

    private void Start(){
        tipObj = transform.Find("Bottom").gameObject;
        tipObj.SetActive(false);
    }

    public void showTip(string contents, TipStyle style){
        switch(style){
            case TipStyle.Bottom:
                tipObj.SetActive(true);
                tipObj.transform.Find("Content").GetComponent<Text>().text = contents;
                break;
        }
    }

    public void hideTip(TipStyle style){
        switch(style){
            case TipStyle.Bottom:
                tipObj.SetActive(false);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLighting : MonoBehaviour
{
    Damage damage;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        damage = transform.GetComponent<Damage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(){
        gameObject.SetActive(true);
        Invoke("Hide",2);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.name == "Player"){
            damage.onDamage(col.gameObject);
        }
    }
}

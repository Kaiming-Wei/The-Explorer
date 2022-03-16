using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillar : MonoBehaviour
{
    const int numFrags = 7;
    public GameObject[] pillarFragment = new GameObject[numFrags];
    public GameObject pillars;
    public AudioClip destructionSound;
    private AudioSource audiosource;


    void Start()
    {
        for (int i = 0; i < numFrags; ++i)
            pillarFragment[i].SetActive(false);

        audiosource = GetComponent<AudioSource>();
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < numFrags; ++i)
            pillarFragment[i].SetActive(false);
        pillars.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            audiosource.PlayOneShot(destructionSound, 0.5f);

            pillars.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0,0);
            pillars.GetComponent<Collider2D>().isTrigger = true;

            for (int i = 0; i < numFrags; ++i)
                pillarFragment[i].SetActive(true);

            StartCoroutine(Delay());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AttackRange"))
        {
            audiosource.PlayOneShot(destructionSound, 0.5f);

            pillars.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            pillars.GetComponent<Collider2D>().isTrigger = true;

            for (int i = 0; i < numFrags; ++i)
                pillarFragment[i].SetActive(true);

            StartCoroutine(Delay());
        }
    }
}

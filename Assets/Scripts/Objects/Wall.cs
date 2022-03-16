using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    const int numFrags = 5;
    public GameObject[] wallFragment = new GameObject[numFrags];
    public GameObject walls;
    public AudioClip destructionSound;
    private AudioSource audiosource;


    void Start()
    {
        for (int i = 0; i < numFrags; ++i)
            wallFragment[i].SetActive(false);

        audiosource = GetComponent<AudioSource>();
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < numFrags; ++i)
            wallFragment[i].SetActive(false);
        walls.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            audiosource.PlayOneShot(destructionSound, 0.5f);

            walls.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            walls.GetComponent<Collider2D>().isTrigger = true;

            for (int i = 0; i < numFrags; ++i)
                wallFragment[i].SetActive(true);

            StartCoroutine(Delay());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AttackRange"))
        {
            audiosource.PlayOneShot(destructionSound, 0.5f);

            walls.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            walls.GetComponent<Collider2D>().isTrigger = true;

            for (int i = 0; i < numFrags; ++i)
                wallFragment[i].SetActive(true);

            StartCoroutine(Delay());
        }
    }
}

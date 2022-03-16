using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;
    public GameObject SwitchDoor;
    public SpriteRenderer switchRenderer;
    public Sprite switchSprite;
    public AudioSource switchSound;
    private Transform tf;
    private bool cameraOn = false;

    private bool enabled = false;
    private bool switchActivated = false;

    void Start()
    {
        SwitchDoor.GetComponent<Rigidbody2D>().isKinematic = true;
        tf = SwitchDoor.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (SwitchDoor.transform.position != endPos)
            {
                SwitchDoor.transform.position = Vector3.MoveTowards(SwitchDoor.transform.position, endPos, speed * Time.deltaTime);
                Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(tf);
            }
            else
            {
                if (SwitchDoor.tag == "Box")
                {
                    SwitchDoor.GetComponent<Rigidbody2D>().isKinematic = false;
                    enabled = false;
                }
                else
                {
                    SwitchDoor.SetActive(false);
                    enabled = false;
                }
                StartCoroutine(Delay());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AttackRange") && !switchActivated)
        {
            switchActivated = true;
            switchRenderer.sprite = switchSprite;
            enabled = true;
            switchSound.Play();
            SwitchDoor.GetComponent<AudioSource>().Play();
        }

        if (other.CompareTag("Bullet") && !switchActivated)
        {
            switchActivated = true;
            switchRenderer.sprite = switchSprite;
            enabled = true;
            switchSound.Play();
            SwitchDoor.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);

        Camera.main.GetComponent<CameraFollowTarget>().SetFollowTarget(GameObject.Find("Player").transform);
    }
}

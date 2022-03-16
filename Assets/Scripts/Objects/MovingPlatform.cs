using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;
    public int movingDirection;
    private bool isEnd = true;

    Rigidbody2D rigid;
    public ContactFilter2D filter;
    ContactPoint2D[] contacts = new ContactPoint2D[10];

    void Start()
    {
        rigid = transform.GetComponent<Rigidbody2D>();
    }

    public void detectedObjects()
    {
        int count = rigid.GetContacts(filter, contacts);

        if(movingDirection == 1)
        {
            for (int i = 0; i < count; ++i)
            {
                contacts[i].rigidbody.velocity += new Vector2(isEnd ? speed : -speed, 0);
            }
        }
        else
        {
            for (int i = 0; i < count; ++i)
            {
                contacts[i].rigidbody.velocity += new Vector2(0, isEnd ? -speed : speed);
                contacts[i].rigidbody.velocity += new Vector2(0, isEnd ? speed : -speed);
            }
        }
    }

    void LateUpdate()
    {
        detectedObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

            if(transform.position == endPos)
            {
                isEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

            if (transform.position == startPos)
            {
                isEnd = true;
            }
        }
    }
}

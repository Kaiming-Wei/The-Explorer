using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftingPlatform : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;
    private bool isEnd = true;

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

            if (transform.position == endPos)
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

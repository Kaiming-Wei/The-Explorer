using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector2 rangeMin;
    public Vector2 rangeMax;

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    public void SetFollowTarget(Transform newtarget){
        target = newtarget;
    }

    public void Follow() {
        if (target == null) {
            return;
        }

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = LimitPos(targetPos);
    }

    public Vector3 LimitPos(Vector3 targetPos) {
        if (targetPos.x < rangeMin.x) {
            targetPos.x = rangeMin.x;
        }
        if (targetPos.y < rangeMin.y) {
            targetPos.y = rangeMin.y;
        }
        if (targetPos.x > rangeMax.x) {
            targetPos.x = rangeMax.x;
        }
        if (targetPos.y > rangeMax.y) {
            targetPos.y = rangeMax.y;
        }
        return targetPos;
    }
}

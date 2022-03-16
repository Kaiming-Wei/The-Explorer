using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    List<GameObject> dcs = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageController dc = collision.transform.GetComponent<DamageController>();
        if (dc != null) {
            dcs.Add(dc.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        DamageController dc = collision.transform.GetComponent<DamageController>();
        if (dc != null) {
            if (!dcs.Contains(dc.gameObject)) {
                dcs.Add(dc.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        DamageController dc = collision.transform.GetComponent<DamageController>();
        if (dc != null) {
            dcs.Remove(dc.gameObject);
        }
    }

    public GameObject[] GetDamageableObjects() {
        return dcs.ToArray();
    }
}

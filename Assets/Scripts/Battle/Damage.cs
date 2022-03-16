using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;

    public void onDamage(GameObject go)
    {
        DamageController dc = go.GetComponent<DamageController>();

        if (dc == null)
        {
            return;
        }

        dc.takeDamage(this.damage);
    }
}

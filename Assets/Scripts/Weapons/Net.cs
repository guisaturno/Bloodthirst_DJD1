using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : WeaponClass
{

    protected override void Start()
    {
        weaponName = "Net";
        weaponDamage = 5f;
        weaponDefense = 25f;
        weaponDurability = 100f;
    }

    public void SpecialAttack()
    {
        SpecialNet = true;
        //Attack();
    }
}

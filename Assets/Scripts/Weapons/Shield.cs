using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : WeaponClass
{

    protected override void Start()
    {
        weaponName = "Shield";
        weaponDamage = 0f;
        weaponDefense = 25f;
        weaponDurability = 100f;
    }

    public void SpecialAttack()
    {
        SpecialShield = true;
        //Attack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : WeaponClass
{
    protected override void Start()
    {
        weaponName = "Sword";
        weaponDamage = 20f;
        weaponDefense = 15f;
        weaponDurability = 100f;
    }

    private void SpecialAttack()
    {
        weaponDamage = (weaponSide == WeaponSide.Left) ? (weaponDamage * 0.5f) + weaponDamage : 20f;
        //Attack();
    }
}

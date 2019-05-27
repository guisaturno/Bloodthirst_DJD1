using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponClass
{
    protected override void Start()
    {
        weaponName = "Sword";
        weaponDamage = (HorizontalAttack == true) ? 10f : 20f;
        weaponDefense = 15f;
        weaponDurability = 100f;
    }

    public void SpecialAttack()
    {
        weaponDamage = (weaponSide == WeaponSide.Left) ? (weaponDamage * 0.5f) + weaponDamage : 20f;
        //Attack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trident : WeaponClass
{
    protected override void Start()
    {
        weaponName = "Trident";
        weaponDamage = (HorizontalAttack == true) ? 10f : 20f;
        weaponDefense = 25f;
        weaponDurability = 100f;
    }
}

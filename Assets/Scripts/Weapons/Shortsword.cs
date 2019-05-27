using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortsword : WeaponClass
{

    protected override void Start()
    {
        weaponName = "Shortsword";
        weaponDamage = (HorizontalAttack == true) ? 10f : 20f;
        weaponDefense = 25f;
        weaponDurability = 100f;
    }
}

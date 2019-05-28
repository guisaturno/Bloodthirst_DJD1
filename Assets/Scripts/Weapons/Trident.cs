using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trident : WeaponClass
{
    protected override void Start()
    {
        weaponName = "Trident";
        weaponDefense = 25f;
        weaponDurability = 100f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortsword : WeaponClass
{

    protected override void Start()
    {
        weaponName = "Shortsword";
        weaponDefense = 25f;
        weaponDurability = 100f;
    }
}

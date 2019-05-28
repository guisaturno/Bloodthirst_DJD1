using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : WeaponClass
{

    protected override void Start()
    {
        
        weaponDefense = 25f;
    }

    internal override void SpecialAttack()
    {
        base.SpecialAttack();
    }
}

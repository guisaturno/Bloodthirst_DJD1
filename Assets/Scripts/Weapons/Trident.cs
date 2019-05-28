using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trident : WeaponClass
{
    protected override void Start()
    {
        weaponDefense = 25f;
    }

    internal override void VerticalAttack()
    {
        base.VerticalAttack();
    }

    internal override void HorizontalAttack()
    {
        base.HorizontalAttack();
    }
}

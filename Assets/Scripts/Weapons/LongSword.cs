using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : WeaponClass
{
    protected override void Start()
    {
        
        weaponDefense = 15f;
    }
    internal override void VerticalAttack()
    {
        base.VerticalAttack();
        print("CalledAttack");
    }

    internal override void HorizontalAttack()
    {
        base.HorizontalAttack();
    }
}

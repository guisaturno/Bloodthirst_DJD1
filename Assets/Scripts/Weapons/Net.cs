using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : WeaponClass
{
    [Header("NetSpecial")]
    [SerializeField] private float netPullSpeed = 100f;
    [SerializeField] private float netPullDistance = 10f;

    Vector2 characterTarget;
    private Transform characterHit;
    private bool netHit;

    protected override void Start()
    {
    }

    internal override void SpecialAttack()
    {
        print("NetSpecial");
       
        if (netHit)
        {
            if (characterHit.position.x > transform.position.x)
            {
                characterTarget = new Vector2(characterHit.position.x - netPullDistance, characterHit.position.y);
            }
            else
            {
                characterTarget = new Vector2(characterHit.position.x + netPullDistance, characterHit.position.y);
            }
            netHit = false;
        }
        if (!netHit)
        {
            characterHit.position = Vector2.MoveTowards(transform.position, characterTarget, netPullSpeed * Time.fixedDeltaTime);
        }
        base.SpecialAttack();
    }

    protected override void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
        {
            characterHit = obj.gameObject.GetComponent<Transform>();
            netHit = true;
        }
        base.OnTriggerEnter2D(obj);
    }
}

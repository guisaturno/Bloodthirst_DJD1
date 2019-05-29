using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : WeaponClass
{
    [Header("NetSpecial")]
    [SerializeField] private float netPullSpeed = 100f;
    [SerializeField] private float netPullDistance = 10f;

    Vector2 characterTarget;
    private GameObject characterHit;
    private bool netHit;

    protected override void Start()
    {
    }

    internal override void SpecialAttack()
    {
        if (characterHit != null && specialRecovery <= 0.1f)
        {
            characterHit.GetComponent<CapsuleCollider2D>().enabled = true;
            characterHit.transform.position = characterTarget;
            characterHit = null;
            print("NetEnd");
        }

        base.SpecialAttack();

        if (netHit && specialRecovery > 0 && characterHit != null)
        {
            if (characterHit.transform.position.x > transform.position.x)
            {
                characterTarget = new Vector2(characterHit.transform.position.x - netPullDistance, characterHit.transform.position.y);
            }
            else
            {
                characterTarget = new Vector2(characterHit.transform.position.x + netPullDistance, characterHit.transform.position.y);
            }
            characterHit.GetComponent<CapsuleCollider2D>().enabled = false;
            netHit = false;
        }
        else if (characterHit != null)
        {
            characterHit.transform.position = Vector2.MoveTowards(transform.position, characterTarget, netPullSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
        {
            characterHit = obj.gameObject;
            netHit = true;
        }
        base.OnTriggerEnter2D(obj);
    }
}

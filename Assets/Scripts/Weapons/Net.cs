using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : WeaponClass
{
    [Header("NetSpecial")]
    [SerializeField] private float netPullSpeed = 100f;
    [SerializeField] private float netPullDistance = 10f;

    Vector2 characterTarget;
    internal GameObject characterHit;
    internal bool netHit;

    protected override void Start()
    {
    }

    internal override void SpecialAttack()
    {
        //Verifies if characterHit isnt null and if action is in recovery
        if (characterHit != null && specialRecovery <= 0.1f)
        {
            //Enable collider
            characterHit.GetComponent<CapsuleCollider2D>().enabled = true;
            //Finish movement
            characterHit.transform.position = characterTarget;
            //Set variable back to null
            characterHit = null;
        }
        //Call base method at WeaponClass
        base.SpecialAttack();
        //Verifies if net hit something and if characterHit isnt null
        if (netHit && specialRecovery > 0 && characterHit != null)
        {
            //Set collided character direction
            if (characterHit.transform.position.x > transform.position.x)
            {
                //Set caracter target moving point
                characterTarget = new Vector2(characterHit.transform.position.x - netPullDistance, characterHit.transform.position.y);
            }
            else
            {
                //Set caracter target moving point
                characterTarget = new Vector2(characterHit.transform.position.x + netPullDistance, characterHit.transform.position.y);
            }
            //Disable collided character collider
            characterHit.GetComponent<CapsuleCollider2D>().enabled = false;
            //Confirm that hit ended
            netHit = false;
        }
        //Verifies if characterHit isnt null
        else if (characterHit != null)
        {
            //Move collided character to target moving point
            characterHit.transform.position = Vector2.MoveTowards(transform.position, characterTarget, netPullSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D obj)
    {
        //Verifies if obj is an enemy or player
        if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
        {
            //Set characterHit as obj
            characterHit = obj.gameObject;
            //Confirm hit
            netHit = true;
        }
        //Call base method at WeaponClass
        base.OnTriggerEnter2D(obj);
    }
}

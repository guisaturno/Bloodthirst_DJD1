using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : WeaponClass
{
    [Header("ShieldSpecial")]
    [SerializeField] private float shieldPushSpeed = 100f;
    [SerializeField] private float shieldPushDistance = 20f;

    Vector2 characterTarget;
    internal GameObject characterHit;
    internal bool shieldHit;

    protected override void Start()
    {
    }

    internal override void SpecialAttack()
    {
        //Verifies if characterHit isnt null and if action is in recovery
        if (characterHit != null && specialRecovery <= 0.1f)
        {
            //Finish movement
            characterHit.transform.position = characterTarget;
            //Set variable back to null
            characterHit = null;

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        //Call base method at WeaponClass
        base.SpecialAttack();

        //Verifies if shield hit something and if characterHit isnt null
        if (shieldHit && specialRecovery > 0 && characterHit != null)
        {
            //Set collided character direction
            if (characterHit.transform.position.x > transform.position.x)
            {
                //Set caracter target moving point
                characterTarget = new Vector2(characterHit.transform.position.x + shieldPushDistance, characterHit.transform.position.y);
            }
            else
            {
                //Set caracter target moving point
                characterTarget = new Vector2(characterHit.transform.position.x - shieldPushDistance, characterHit.transform.position.y);
            }
            //Confirm that hit ended
            shieldHit = false;
        }
        //Verifies if characterHit isnt null
        else if (characterHit != null)
        {
            //Move collided character to target moving point
            characterHit.transform.position = Vector2.MoveTowards(transform.position, characterTarget, shieldPushSpeed * Time.fixedDeltaTime);
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
            shieldHit = true;
        }
        //Call base method at WeaponClass
        base.OnTriggerEnter2D(obj);
    }
}

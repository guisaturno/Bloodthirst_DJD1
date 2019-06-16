using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClass : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] internal float baseDamage;

    internal float damage;

    [Header("SpecialAttack")]
    [SerializeField] internal float specialRecoveryTime = 1.0f;
    [SerializeField] private float pushDistance;

    internal bool attacked = true;
    internal bool hit;

    private void Start()
    {
        damage = baseDamage;
    }

    internal void VerticalAttack()
    {
        if (attacked)
        {
            //Set damage
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            damage = damage * 2;
            //Confirm that character attacked
            attacked = false;
        }
    }

    internal void HorizontalAttack()
    {
        if (attacked)
        {
            //Confirm that character attacked
            attacked = false;
        }
    }

    internal void SpecialAttack()
    {
        if (attacked)
        {
            //Set damage
            if (gameObject.name != "Shield")
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //Confirm that character attacked
            attacked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (hit && obj.CompareTag("Player") || hit && obj.CompareTag("Enemy"))
        {
            obj.gameObject.GetComponent<Character>().TakeDamage(damage, pushDistance, transform);
            hit = false;
        }
    }
}

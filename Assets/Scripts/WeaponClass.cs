using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClass : MonoBehaviour
{
    [Header("SpecialAttack")]
    [SerializeField] protected float specialRecoveryTime = 1.0f;

    protected float specialRecovery;

    [Header("HorizontalAttack")]
    [SerializeField] private float horizontalRecoveryTime = 1.0f;
    [SerializeField] private float horizontalDistance = 15;

    private float horizontalRecovery;

    [Header("VerticalAttack")]
    [SerializeField] private float verticalRecoveryTime = 1.0f;

    private float verticalRecovery;
    private bool attacked = true;
    private bool hit;

    [Header("To be review")]
    [SerializeField] protected float damage;

    protected virtual void Start()
    {
    }

    internal virtual void VerticalAttack()
    {
        //Discount recovery time
        verticalRecovery -= Time.fixedDeltaTime;
        if (attacked)
        {
            //Set damage
            damage = damage * 2;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //Reset vertical attack recovery time
            verticalRecovery = verticalRecoveryTime;
            //Confirm that character attacked
            attacked = false;
        }

        if (verticalRecovery <= 0.0f)
        {
            //Reset variables
            if (transform.parent.parent.tag == "Player")
            {
                print("!");
                gameObject.GetComponentInParent<Player>().state = Character.State.Idle;
                gameObject.GetComponentInParent<Player>().attacked = true;
            }
            else if ((transform.parent.parent.tag == "Enemy"))
            {
                gameObject.GetComponentInParent<EnemyAI>().state = Character.State.Idle;
                gameObject.GetComponentInParent<EnemyAI>().attacked = true;
            }

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            verticalRecovery = 0.0f;
            damage = damage / 2;
            attacked = true;
            hit = true;
        }
    }

    internal virtual void HorizontalAttack()
    {
        //Discount recovery time
        horizontalRecovery -= Time.fixedDeltaTime;
        if (attacked)
        {
            //Set damage
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //Reset vertical attack recovery time
            horizontalRecovery = horizontalRecoveryTime;
            //Confirm that character attacked
            attacked = false;
        }

        if (horizontalRecovery <= 0.0f)
        {
            //Reset variables
            if (transform.parent.parent.tag == "Player")
            {
                gameObject.GetComponentInParent<Player>().state = Character.State.Idle;
                gameObject.GetComponentInParent<Player>().attacked = true;
            }
            else if (transform.parent.parent.tag == "Enemy")
            {
                gameObject.GetComponentInParent<EnemyAI>().state = Character.State.Idle;
                gameObject.GetComponentInParent<EnemyAI>().attacked = true;
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            horizontalRecovery = 0.0f;
            attacked = true;
            hit = true;
        }
    }

    internal virtual void SpecialAttack()
    {
        //Discount recovery time
        specialRecovery -= Time.fixedDeltaTime;
        if (attacked)
        {
            //Set damage
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //Reset special attack recovery time
            specialRecovery = specialRecoveryTime;
            //Confirm that character attacked
            attacked = false;
        }

        if (specialRecovery <= 0.0f)
        {
            print("Time end!!!");
            //Reset variables
            if (transform.parent.parent.tag == "Player")
            {
                gameObject.GetComponentInParent<Player>().state = Character.State.Idle;
                gameObject.GetComponentInParent<Player>().attacked = true;
            }
            else if (transform.parent.parent.tag == "Enemy")
            {
                gameObject.GetComponentInParent<EnemyAI>().state = Character.State.Idle;
                gameObject.GetComponentInParent<EnemyAI>().attacked = true;
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            specialRecovery = 0.0f;
            attacked = true;
            hit = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D obj)
    {
        if (hit && obj.CompareTag("Player") || hit && obj.CompareTag("Enemy"))
        {
            obj.gameObject.GetComponent<Character>().TakeDamage(damage);
            hit = false;
            print(obj.name);
        }
    }

    //    public void Attack()
    //    {
    //        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(weaponSensor.position, weaponRange, whatisEnemy);

    //        for (int i = 0; i < enemiesHit.Length; i++)
    //        {
    //            enemiesHit[i].GetComponent<Character>().TakeDamage(weaponDamage); // metodo na classe enemy
    //            if (HorizontalAttack)
    //            {
    //                if (dashTime <= 0)
    //                {
    //                    dashTime = startDashTime;
    //                    rb.velocity = Vector2.zero;
    //                }
    //                else
    //                {
    //                    dashTime -= Time.deltaTime;
    //                    if (character.transform.right.x > 0f)
    //                        rb.velocity = Vector2.right * dashSpeed;
    //                    else if (character.transform.right.x < 0f)
    //                        rb.velocity = Vector2.left * dashSpeed;
    //                }
    //            }
    //            Debug.Log("HIT");
    //        }
    //    }

    //    public void ResultAttack(WeaponClass other)
    //    {
    //        weaponDurability -= Mathf.Max(other.weaponDamage - weaponDefense, 0);
    //        CheckBroken();
    //    }


    //    protected void CheckBroken()
    //    {
    //        weaponBroke = (weaponDurability <= 0);
    //    }
}

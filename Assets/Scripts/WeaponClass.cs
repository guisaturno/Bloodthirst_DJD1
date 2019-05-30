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

    internal float specialRecovery;

    [Header("HorizontalAttack")]
    [SerializeField] internal float horizontalRecoveryTime = 1.0f;

    internal float horizontalRecovery;

    [Header("VerticalAttack")]
    [SerializeField] internal float verticalRecoveryTime = 1.0f;

    internal float verticalRecovery;
    internal bool attacked = true;
    internal bool hit;

    protected virtual void Start()
    {
        damage = baseDamage;
    }

    internal virtual void VerticalAttack()
    {
        //Discount recovery time
        verticalRecovery -= Time.fixedDeltaTime;
        if (attacked)
        {
            //Set damage
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            damage = damage * 2;
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
                gameObject.GetComponentInParent<Player>().attacked = true;
            }
            else if (transform.parent.parent.tag == "Enemy")
            {
                gameObject.GetComponentInParent<EnemyAI>().attacked = true;
            }
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
            if (gameObject.name != "Shield")
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //Reset special attack recovery time
            specialRecovery = specialRecoveryTime;
            //Confirm that character attacked
            attacked = false;
        }

        if (specialRecovery <= 0.0f)
        {
            //Reset variables
            if (transform.parent.parent.tag == "Player")
            {
                if (gameObject.name != "Shield")
                {
                    gameObject.GetComponentInParent<Player>().state = Character.State.Idle;
                }
                gameObject.GetComponentInParent<Player>().attacked = true;
            }
            else if (transform.parent.parent.tag == "Enemy")
            {
                if (gameObject.name != "Shield")
                {
                    gameObject.GetComponentInParent<EnemyAI>().state = Character.State.Idle;
                }
                gameObject.GetComponentInParent<EnemyAI>().attacked = true;
            }
            if (gameObject.name != "Shield")
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
            Character.FacingDirection facingDirection
            = transform.rotation == Quaternion.Euler(0.0f, 180.0f, 0.0f)
            ? Character.FacingDirection.left : Character.FacingDirection.right;

            obj.gameObject.GetComponent<Character>().TakeDamage(damage, facingDirection,transform.position.x);
            hit = false;
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

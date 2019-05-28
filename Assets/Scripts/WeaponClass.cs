using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClass : MonoBehaviour
{
    [Header ("Weapon Class")]
    [SerializeField] protected float weaponRange;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float startDashTime;

    protected string weaponName = "Weapon";
    protected float weaponDamage = 50;
    protected float weaponDefense = 50;
    protected float weaponDurability = 100;
    protected bool weaponBroke = false;
    private Transform character;
    public WeaponSide weaponSide;
    public bool SpecialShield { get; set; } = false;
    public bool SpecialNet { get; set; } = false;
    protected float dashTime;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        character = GetComponent<Character>().transform;
        rb = GameObject.FindGameObjectWithTag("Character").GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    protected virtual void VerticalAttack()
    {

    }

    protected virtual void HorizontalAttack()
    {

    }

    protected virtual void SpecialAttack()
    {

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

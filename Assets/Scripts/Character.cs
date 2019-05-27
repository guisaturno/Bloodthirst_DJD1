using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponSide { Left, Right }

public class Character : MonoBehaviour
{
    [Header("Character")]

    // Variables
    [SerializeField] protected float walkSpeed = 2500f;
    [SerializeField] protected float maxRollSpeed = 500f;
    [SerializeField] protected float startTimeRolling;
    [SerializeField] protected float rollSpeedMultiplier = 13f;
    [SerializeField] protected float climbSpeed = 2500f;
    [SerializeField] protected float startTimeClimbing;
    [SerializeField] protected float startClimbDistance = 30f;
    [SerializeField] protected float startTimeAttack;
    [SerializeField] protected GameObject leftWeapon;
    [SerializeField] protected GameObject rightWeapon;
    [SerializeField] protected float knockback;
    [SerializeField] protected float knockbackCount;
    [SerializeField] protected bool knockbackRight;
    [SerializeField] protected float startStunTime;


    public float MaxHP { get; set; } = 100;
    public float CurrentHP { get; set; }
    protected float rollSpeed = 500f;
    protected float timeRolling = 0f;
    protected float timeClimbing = 0f;
    private float climbDistance;
    protected bool canClimb = true;
    protected float timeAttacking = 0f;
    protected WeaponClass instance;
    protected float stunTime;


    protected Animator animator;
    protected Rigidbody2D rb;
    protected Vector2 currentVelocity;

    protected enum State { Idle, Roll, Climb, Attack, Run, Defend }
    protected State state;
    public WeaponSide weaponSide;

    // Properties
    bool IsInvunerable
    {
        get
        {
            if (state == State.Roll) return true;

            return false;
        }
    }

    // Methods
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHP = MaxHP;
        climbDistance = startClimbDistance;
        stunTime = startStunTime;
    }

    protected void RotateCharacter()
    {
        transform.rotation = (transform.right.x > 0.0f) ?
            Quaternion.Euler(0.0f, 180.0f, 0.0f) : Quaternion.identity;
    }

    protected void RollCharacter()
    {
        // Process of rolling
        if (transform.right.x > 0.0f)
        {
            transform.position += new Vector3(1, 0) * rollSpeed * Time.fixedDeltaTime;
        }
        else if (transform.right.x < 0.0f)
        {
            transform.position -= new Vector3(1, 0) * rollSpeed * Time.fixedDeltaTime;
        }

        rollSpeed -= rollSpeed * rollSpeedMultiplier * Time.fixedDeltaTime;

        if (rollSpeed < 5f)
        {
            state = State.Run;
        }
    }

    protected void ClimbCharacter()
    {
        if (climbDistance <= 0)
        {
            animator.SetBool("Climb", false);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            state = State.Run;
            climbDistance = startClimbDistance;
            currentVelocity = Vector2.zero;
        }
        else
        {
            animator.SetBool("Climb", true);
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            climbDistance -= Time.fixedDeltaTime;
            currentVelocity = (canClimb == false) ? Vector2.up * climbSpeed * Time.fixedDeltaTime : Vector2.down * climbSpeed * Time.fixedDeltaTime;
        }
    }

    protected void RightWeaponAttack()
    {

        switch (rightWeapon.name)
        {
            case "Sword":
                weaponSide = WeaponSide.Right;
               // rightWeapon.GetComponent<Sword>().Attack();
                break;
            case "Trident":
               // rightWeapon.GetComponent<Trident>().Attack();
                break;
            case "LongSword":
               // rightWeapon.GetComponent<LongSword>().Attack();
                break;
            default:
                break;
        }
    }

    protected void LeftWeaponAttack()
    {
        switch (leftWeapon.name)
        {
            case "Sword":
                weaponSide = WeaponSide.Left;
               // leftWeapon.GetComponent<Sword>().SpecialAttack();
                break;
            case "Shield":
               // leftWeapon.GetComponent<Shield>().SpecialAttack();
                break;
            case "Net":
              //  leftWeapon.GetComponent<Net>().SpecialAttack();
                break;
            default:
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (instance.SpecialShield)
        {
            instance.SpecialShield = false;
            if (knockbackCount > 0)
            {
                rb.velocity = (knockbackRight == true) ? new Vector2(-knockback, 0) : new Vector2(knockback, 0);
                knockback -= Time.deltaTime;
            }
        }
        else if (instance.SpecialNet)
        {
            instance.SpecialNet = false;
            if (stunTime <= 0)
            {
                walkSpeed = 5000f;
                stunTime = startStunTime;
            }
            else
            {
                walkSpeed = 0;
                stunTime -= Time.deltaTime;
            }
        }
    }
}

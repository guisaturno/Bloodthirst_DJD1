using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponSide { Left, Right }

public class Character : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] protected Animator characterAnim;
    [SerializeField] protected Animator leftWeaponAnim;
    [SerializeField] protected Animator rightWeaponAnim;

    [Header("Roll")]
    [SerializeField] private float rollDistance = 300.0f;
    [SerializeField] private float rollRecoveryTime = 10.0f;
    [SerializeField] private float rollSpeed = 15.0f;

    private Vector2 rollTarget;
    private float rollRecovery;
    private bool rolled = true;

    [Header("Move")]
    [SerializeField] private Transform rightMovePoint;
    [SerializeField] private Transform leftMovePoint;
    [SerializeField] private float moveSpeed = 15f;

    private Vector2 rightMoveTarget;
    private Vector2 leftMoveTarget;

    [Header("Climb")]
    [SerializeField] private float climbDistance = 300.0f;
    [SerializeField] private float climbRecoveryTime = 10.0f;
    [SerializeField] protected float climbSpeed = 15.0f;

    private Vector2 climbTarget;
    private float climbRecovery;
    private bool climbed = true;



    [Header("To be review")]
    // Variables
    [SerializeField] protected float walkSpeed = 2500f;
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
    protected float timeRolling = 0f;
    protected float timeClimbing = 0f;
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

        //To be review
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHP = MaxHP;
        climbDistance = startClimbDistance;
        stunTime = startStunTime;
    }

    protected virtual void Update()
    {
        print(state);
        //Set animator booleans to false according to state
        switch (state)
        {
            case State.Climb:
                characterAnim.SetBool("Run", false);
                leftWeaponAnim.SetBool("Run", false);
                rightWeaponAnim.SetBool("Run", false);

                characterAnim.SetBool("Defense", false);
                leftWeaponAnim.SetBool("Defense", false);
                rightWeaponAnim.SetBool("Defense", false);
                break;
            case State.Run:
                characterAnim.SetBool("Defense", false);
                leftWeaponAnim.SetBool("Defense", false);
                rightWeaponAnim.SetBool("Defense", false);

                characterAnim.SetBool("Climb", false);
                leftWeaponAnim.SetBool("Climb", false);
                rightWeaponAnim.SetBool("Climb", false);
                break;
            case State.Defend:
                characterAnim.SetBool("Run", false);
                leftWeaponAnim.SetBool("Run", false);
                rightWeaponAnim.SetBool("Run", false);

                characterAnim.SetBool("Climb", false);
                leftWeaponAnim.SetBool("Climb", false);
                rightWeaponAnim.SetBool("Climb", false);
                break;
            default:
                characterAnim.SetBool("Climb", false);
                leftWeaponAnim.SetBool("Climb", false);
                rightWeaponAnim.SetBool("Climb", false);

                characterAnim.SetBool("Run", false);
                leftWeaponAnim.SetBool("Run", false);
                rightWeaponAnim.SetBool("Run", false);

                characterAnim.SetBool("Defense", false);
                leftWeaponAnim.SetBool("Defense", false);
                rightWeaponAnim.SetBool("Defense", false);
                break;
        }
        switch (state)
        {
            case State.Idle:
                break;
            case State.Roll:
                RollCharacter();
                break;
            case State.Climb:
                ClimbCharacter();
                break;
            case State.Attack:
                break;
            case State.Run:
                if (transform.rotation.y == 0.0f)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
                break;
            case State.Defend:
                break;
            default:
                break;
        }
    }

    protected void MoveRight()
    {
        //Animation
        characterAnim.SetBool("Run", true);
        leftWeaponAnim.SetBool("Run", true);
        rightWeaponAnim.SetBool("Run", true);
        //Set character movement direction
        rightMoveTarget = new Vector2(rightMovePoint.position.x, transform.position.y);
        //Move character towards right
        transform.position = Vector2.MoveTowards(transform.position, rightMoveTarget, moveSpeed * Time.fixedDeltaTime);
    }

    protected void MoveLeft()
    {
        //Animation
        characterAnim.SetBool("Run", true);
        leftWeaponAnim.SetBool("Run", true);
        rightWeaponAnim.SetBool("Run", true);
        //Set character movement direction
        leftMoveTarget = new Vector2(leftMovePoint.position.x, transform.position.y);
        //Move character towards left
        transform.position = Vector2.MoveTowards(transform.position, leftMoveTarget, moveSpeed * Time.fixedDeltaTime);
    }

    protected void RotateCharacter()
    {
        transform.rotation = (transform.right.x > 0.0f) ?
            Quaternion.Euler(0.0f, 180.0f, 0.0f) : Quaternion.identity;
    }

    protected void RollCharacter()
    { 
        //Set roll distance direction
        rollDistance = (transform.rotation.y == 0.0f) ? rollDistance : -rollDistance;
        //Discount recovery time
        rollRecovery -= Time.fixedDeltaTime;

        if (rolled)
        {
            //Animation
            characterAnim.SetTrigger("Roll");
            leftWeaponAnim.SetTrigger("Roll");
            rightWeaponAnim.SetTrigger("Roll");
            //Set roll direction
            rollTarget = new Vector2(transform.position.x + rollDistance, transform.position.y);
            //Reset roll recovery time
            rollRecovery = rollRecoveryTime + 5.0f;
            //Confirm that character rolled
            rolled = false;
        }
        else if (transform.position.x != rollTarget.x)
        {
            //Move character
            this.transform.position = Vector2.MoveTowards(transform.position, rollTarget, rollSpeed * Time.fixedDeltaTime);
        }

        if (rollRecovery <= 5.0f)
        {
            //Reset variables
            state = State.Idle;
            rollRecovery = 0.0f;
            rolled = true;
        }
    }

    protected void ClimbCharacter()
    {
        //Set climb distance direction
        climbDistance = (transform.position.y == -58.0f) ? climbDistance : -climbDistance;
        //Discount recovery time
        climbRecovery -= Time.fixedDeltaTime;

        if (climbed)
        {
            //Animation
            characterAnim.SetBool("Climb", true);
            leftWeaponAnim.SetBool("Climb", true);
            rightWeaponAnim.SetBool("Climb", true);
            //Set climb direction
            climbTarget = new Vector2(transform.position.x , transform.position.y + climbDistance);
            //Reset climb recovery time
            climbRecovery = climbRecoveryTime + 5.0f;
            //Confirm that character climbed
            rolled = false;
        }
        else if (transform.position.y != rollTarget.y)
        {
            //Move character
            transform.position = Vector2.MoveTowards(transform.position, climbTarget, climbSpeed * Time.fixedDeltaTime);
        }

        if (climbRecovery <= 5.0f)
        {
            //Reset variables
            state = State.Idle;
            climbRecovery = 0.0f;
            climbed = true;
        }

        //if (climbDistance <= 0)
        //{
        //    animator.SetBool("Climb", false);
        //    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        //    state = State.Run;
        //    climbDistance = startClimbDistance;
        //    currentVelocity = Vector2.zero;
        //}
        //else
        //{
        //    animator.SetBool("Climb", true);
        //    rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        //    climbDistance -= Time.fixedDeltaTime;
        //    currentVelocity = (canClimb == false) ? Vector2.up * climbSpeed * Time.fixedDeltaTime : Vector2.down * climbSpeed * Time.fixedDeltaTime;
        //}
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

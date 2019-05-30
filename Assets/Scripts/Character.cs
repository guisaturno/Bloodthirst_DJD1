using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //ENUMS
    internal enum State { Idle, Roll, Climb, Attack, Run, Defend, Stun }
    protected enum AttackState { Horizontal, Vertical, Special }

    internal State state;
    protected AttackState attackState;

    [Header("Animator")]
    [SerializeField] protected Animator characterAnim;
    [SerializeField] protected Animator leftWeaponAnim;
    [SerializeField] protected Animator rightWeaponAnim;

    [Header("Attack")]
    [SerializeField] protected GameObject leftWeapon;
    [SerializeField] protected GameObject rightWeapon;

    internal bool attacked = true;
    
    [Header("Dash")]
    [SerializeField] private float dashRecoveryTime = 1f;
    [SerializeField] private float dashDistance = 30.0f;
    [SerializeField] private float dashSpeed = 15.0f;

    private Vector2 dashTarget;
    private float dashRecovery;
    private bool dashed = true;

    [Header("Roll")]
    [SerializeField] private float rollDistance = 30.0f;
    [SerializeField] private float rollRecoveryTime = 10.0f;
    [SerializeField] private float rollSpeed = 15.0f;

    private Vector2 rollTarget;
    private float rollRecovery;
    private bool rolled = true;
    protected bool facingRight = true;

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
    private bool atGround = true;

    [Header("Stun")]
    [SerializeField]private float stunRecoveryTime;

    private bool stuned = true;
    private float stunRecovery;
    

    //Defend
    private bool isDefending;
    internal enum FacingDirection { right, left }
    internal FacingDirection facingDirection;


    public float MaxHP { get; set; } = 100;
    public float CurrentHP { get; set; }

    protected Animator animator;
    protected Rigidbody2D rb;
    protected Vector2 currentVelocity;

    // Methods
    protected virtual void Start()
    {
        //To be review
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHP = MaxHP;
    }

    protected virtual void Update()
    {
        //Set facing direction
        facingDirection = transform.rotation == Quaternion.Euler(0.0f, 180.0f, 0.0f)
            ? FacingDirection.left : FacingDirection.right;
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

                characterAnim.SetBool("HorizontalAttack", false);
                leftWeaponAnim.SetBool("HorizontalAttack", false);
                rightWeaponAnim.SetBool("HorizontalAttack", false);

                characterAnim.SetBool("VerticalAttack", false);
                leftWeaponAnim.SetBool("VerticalAttack", false);
                rightWeaponAnim.SetBool("VerticalAttack", false);

                characterAnim.SetBool("NetAttack", false);
                leftWeaponAnim.SetBool("NetAttack", false);
                rightWeaponAnim.SetBool("NetAttack", false);

                characterAnim.SetBool("ShieldAttack", false);
                leftWeaponAnim.SetBool("ShieldAttack", false);
                rightWeaponAnim.SetBool("ShieldAttack", false);
                break;
            case State.Run:
                characterAnim.SetBool("Defense", false);
                leftWeaponAnim.SetBool("Defense", false);
                rightWeaponAnim.SetBool("Defense", false);

                characterAnim.SetBool("Climb", false);
                leftWeaponAnim.SetBool("Climb", false);
                rightWeaponAnim.SetBool("Climb", false);

                characterAnim.SetBool("HorizontalAttack", false);
                leftWeaponAnim.SetBool("HorizontalAttack", false);
                rightWeaponAnim.SetBool("HorizontalAttack", false);

                characterAnim.SetBool("VerticalAttack", false);
                leftWeaponAnim.SetBool("VerticalAttack", false);
                rightWeaponAnim.SetBool("VerticalAttack", false);

                characterAnim.SetBool("NetAttack", false);
                leftWeaponAnim.SetBool("NetAttack", false);
                rightWeaponAnim.SetBool("NetAttack", false);

                characterAnim.SetBool("ShieldAttack", false);
                leftWeaponAnim.SetBool("ShieldAttack", false);
                rightWeaponAnim.SetBool("ShieldAttack", false);
                break;
            case State.Defend:
                characterAnim.SetBool("Run", false);
                leftWeaponAnim.SetBool("Run", false);
                rightWeaponAnim.SetBool("Run", false);

                characterAnim.SetBool("Climb", false);
                leftWeaponAnim.SetBool("Climb", false);
                rightWeaponAnim.SetBool("Climb", false);

                characterAnim.SetBool("HorizontalAttack", false);
                leftWeaponAnim.SetBool("HorizontalAttack", false);
                rightWeaponAnim.SetBool("HorizontalAttack", false);

                characterAnim.SetBool("VerticalAttack", false);
                leftWeaponAnim.SetBool("VerticalAttack", false);
                rightWeaponAnim.SetBool("VerticalAttack", false);

                characterAnim.SetBool("NetAttack", false);
                leftWeaponAnim.SetBool("NetAttack", false);
                rightWeaponAnim.SetBool("NetAttack", false);

                characterAnim.SetBool("ShieldAttack", false);
                leftWeaponAnim.SetBool("ShieldAttack", false);
                rightWeaponAnim.SetBool("ShieldAttack", false);
                break;
            case State.Idle:
                characterAnim.SetBool("Climb", false);
                leftWeaponAnim.SetBool("Climb", false);
                rightWeaponAnim.SetBool("Climb", false);

                characterAnim.SetBool("Run", false);
                leftWeaponAnim.SetBool("Run", false);
                rightWeaponAnim.SetBool("Run", false);

                characterAnim.SetBool("Defense", false);
                leftWeaponAnim.SetBool("Defense", false);
                rightWeaponAnim.SetBool("Defense", false);

                characterAnim.SetBool("HorizontalAttack", false);
                leftWeaponAnim.SetBool("HorizontalAttack", false);
                rightWeaponAnim.SetBool("HorizontalAttack", false);

                characterAnim.SetBool("VerticalAttack", false);
                leftWeaponAnim.SetBool("VerticalAttack", false);
                rightWeaponAnim.SetBool("VerticalAttack", false);

                characterAnim.SetBool("NetAttack", false);
                leftWeaponAnim.SetBool("NetAttack", false);
                rightWeaponAnim.SetBool("NetAttack", false);

                characterAnim.SetBool("ShieldAttack", false);
                leftWeaponAnim.SetBool("ShieldAttack", false);
                rightWeaponAnim.SetBool("ShieldAttack", false);
                break;
        }
        switch (state)
        {
            case State.Idle:
                ResetCharacter();
                break;
            case State.Stun:
                ResetCharacter();
                Stun();
                break;
            case State.Roll:
                RollCharacter();
                break;
            case State.Climb:
                ClimbCharacter();
                break;
            case State.Attack:
                Attack();
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
                characterAnim.SetBool("Defense", true);
                leftWeaponAnim.SetBool("Defense", true);
                rightWeaponAnim.SetBool("Defense", true);
                isDefending = true;
                break;
            default:
                break;
        }
    }
    protected void ResetCharacter()
    {
        //Weapon
        rightWeapon.GetComponent<BoxCollider2D>().enabled = false;
        leftWeapon.GetComponent<BoxCollider2D>().enabled = false;
        if (rightWeapon.name == "Trident")
        {
            rightWeapon.GetComponent<Trident>().horizontalRecovery = 0.0f;
            rightWeapon.GetComponent<Trident>().verticalRecovery = 0.0f;
            rightWeapon.GetComponent<Trident>().damage = rightWeapon.GetComponent<Trident>().baseDamage;
            rightWeapon.GetComponent<Trident>().hit = true;
            rightWeapon.GetComponent<Trident>().attacked = true;
        }
        else if (rightWeapon.name == "LongSword")
        {
            rightWeapon.GetComponent<LongSword>().horizontalRecovery = 0.0f;
            rightWeapon.GetComponent<LongSword>().verticalRecovery = 0.0f;
            rightWeapon.GetComponent<LongSword>().damage = rightWeapon.GetComponent<LongSword>().baseDamage;
            rightWeapon.GetComponent<LongSword>().hit = true;
            rightWeapon.GetComponent<LongSword>().attacked = true;
        }

        if (leftWeapon.name == "Net")
        {
            leftWeapon.GetComponent<Net>().specialRecovery = 0;
            leftWeapon.GetComponent<Net>().attacked = true;
            leftWeapon.GetComponent<Net>().hit = true;
            leftWeapon.GetComponent<Net>().netHit = false;
            leftWeapon.GetComponent<Net>().characterHit = null;
        }
        else if (leftWeapon.name == "Shield")
        {
            leftWeapon.GetComponent<Shield>().specialRecovery = 0;
            leftWeapon.GetComponent<Shield>().attacked = true;
            leftWeapon.GetComponent<Shield>().hit = true;
            leftWeapon.GetComponent<Shield>().shieldHit = false;
            leftWeapon.GetComponent<Shield>().characterHit = null;
        }
        //Attack
        attacked = true;

        //Dash
        dashRecovery = 0.0f;
        dashed = true;

        //Roll
        rollRecovery = 0.0f;
        rolled = true;

        //Climb
        climbRecovery = 0.0f;
        climbed = true;
        if (atGround)
        {
            transform.position = new Vector2(transform.position.x, -58);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, -17);
        }

        //Defending
        isDefending = false;
    }

    protected void Attack()
    {
        if (attacked)
        {
            switch (attackState)
            {
                case AttackState.Horizontal:
                    //Animation
                    characterAnim.SetBool("HorizontalAttack", true);
                    leftWeaponAnim.SetBool("HorizontalAttack", true);
                    rightWeaponAnim.SetBool("HorizontalAttack", true);
                    Dash();
                    switch (rightWeapon.name)
                    {
                        case "Trident":
                            rightWeapon.GetComponent<Trident>().HorizontalAttack();
                            break;
                        case "LongSword":
                            rightWeapon.GetComponent<LongSword>().HorizontalAttack();
                            break;
                        default:
                            break;
                    }
                    break;
                case AttackState.Vertical:
                    //Animation
                    characterAnim.SetBool("VerticalAttack", true);
                    leftWeaponAnim.SetBool("VerticalAttack", true);
                    rightWeaponAnim.SetBool("VerticalAttack", true);
                    switch (rightWeapon.name)
                    {
                        case "Trident":
                            rightWeapon.GetComponent<Trident>().VerticalAttack();
                            break;
                        case "LongSword":
                            print("LongSword");
                            rightWeapon.GetComponent<LongSword>().VerticalAttack();
                            break;
                        default:
                            break;
                    }
                    break;
                case AttackState.Special:
                    switch (leftWeapon.name)
                    {
                        case "Shield":
                            characterAnim.SetBool("ShieldAttack", true);
                            leftWeaponAnim.SetBool("ShieldAttack", true);
                            rightWeaponAnim.SetBool("ShieldAttack", true);
                            leftWeapon.GetComponent<Shield>().SpecialAttack();
                            Dash();
                            break;
                        case "Net":
                            characterAnim.SetBool("NetAttack", true);
                            leftWeaponAnim.SetBool("NetAttack", true);
                            rightWeaponAnim.SetBool("NetAttack", true);
                            leftWeapon.GetComponent<Net>().SpecialAttack();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void Dash()
    {
        //Discount recovery time
        dashRecovery -= Time.fixedDeltaTime;

        if (dashed)
        {
            //Set roll direction
            if (transform.right.x > 0.0f)
            {
                //Roll right
                dashTarget = new Vector2(transform.position.x + dashDistance, transform.position.y);
            }
            else
            {
                //Roll left
                dashTarget = new Vector2(transform.position.x - dashDistance, transform.position.y);
            }
            //Reset roll recovery time
            dashRecovery = dashRecoveryTime;
            //Confirm that character rolled
            dashed = false;
            //Enable weapon collider
            if (attackState == AttackState.Horizontal)
            {
                rightWeapon.GetComponent<BoxCollider2D>().enabled = true;
            }
            else if (attackState == AttackState.Special)
            {
                leftWeapon.GetComponent<BoxCollider2D>().enabled = true;
            }

        }
        else if (transform.position.x != dashTarget.x)
        {
            //Move character
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.fixedDeltaTime);
        }

        if (dashRecovery <= 0.0f)
        {
            //Reset variables
            state = State.Idle;
            dashRecovery = 0.0f;
            dashed = true;
            //Disable weapon collider
            if (attackState == AttackState.Horizontal)
            {
                rightWeapon.GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (attackState == AttackState.Special)
            {
                leftWeapon.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    private void RollCharacter()
    {
        //Discount recovery time
        rollRecovery -= Time.fixedDeltaTime;

        if (rolled)
        {
            //Disable collider
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            //Animation
            characterAnim.SetTrigger("Roll");
            leftWeaponAnim.SetTrigger("Roll");
            rightWeaponAnim.SetTrigger("Roll");
            //Set roll direction
            if (transform.right.x > 0.0f)
            {
                //Roll right
                rollTarget = new Vector2(transform.position.x + rollDistance, transform.position.y);
            }
            else
            {
                //Roll left
                rollTarget = new Vector2(transform.position.x - rollDistance, transform.position.y);
            }
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
            //Enable collider
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            //Reset variables
            state = State.Idle;
            rollRecovery = 0.0f;
            rolled = true;
        }
    }

    private void ClimbCharacter()
    {
        //Discount recovery time
        climbRecovery -= Time.fixedDeltaTime;

        if (climbed)
        {
            atGround = !atGround;
            //Animation
            characterAnim.SetBool("Climb", true);
            leftWeaponAnim.SetBool("Climb", true);
            rightWeaponAnim.SetBool("Climb", true);
            //Set climb direction
            if (transform.position.y == -58)
            {
                //Climb up
                climbTarget = new Vector2(transform.position.x, transform.position.y + climbDistance);
            }
            else
            {
                //Climb down
                climbTarget = new Vector2(transform.position.x, transform.position.y - climbDistance);
            }
            //Reset climb recovery time
            climbRecovery = climbRecoveryTime + 5.0f;
            //Confirm that character climbed
            climbed = false;
        }
        else if (transform.position.y != climbTarget.y)
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
    }

    private void MoveRight()
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

    private void MoveLeft()
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

    protected void Stun()
    {
        //Discount recovery time
        stunRecovery -= Time.fixedDeltaTime;

        if (stuned)
        {
            stunRecovery = stunRecoveryTime;
            stuned = false;
        }

        if (stunRecovery <= 0.0f)
        {
            print("Stun is over");
            state = State.Idle;
            stuned = true;
        }
    }

    internal void TakeDamage(float damage, FacingDirection hitDirection, float pos)
    {
        if (isDefending && facingDirection == hitDirection)
        {
            characterAnim.SetTrigger("Block");
            leftWeaponAnim.SetTrigger("Block");
            rightWeaponAnim.SetTrigger("Block");
            //if(facingDirection == FacingDirection.right && transform.position.x < pos 
            //|| (facingDirection == FacingDirection.left && transform.position.x > pos))
            //{
            //    characterAnim.SetTrigger("Block");
            //    leftWeaponAnim.SetTrigger("Block");
            //    rightWeaponAnim.SetTrigger("Block");
            //}
        }
        else
        {
            state = State.Stun;
            CurrentHP -= damage;
            characterAnim.SetTrigger("Hit");
            leftWeaponAnim.SetTrigger("Hit");
            rightWeaponAnim.SetTrigger("Hit");
            print(CurrentHP);
        }
    }
}

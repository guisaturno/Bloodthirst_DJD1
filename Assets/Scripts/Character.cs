using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //ENUMS
    internal enum State { Idle, Roll, Climb, Attack, Run, Defend, Stun, Push, Dead }
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
    [SerializeField] private float stunRecoveryTime;

    private bool stuned = true;
    private float stunRecovery;

    [Header("Push")]
    [SerializeField] private float pushSpeed = 100f;
    [SerializeField] private float pushDistance = 10f;
    [SerializeField] internal float pushRecoveryTime = 1.0f;

    internal float pushRecovery;
    Vector2 characterTarget;
    internal GameObject characterHit;
    internal Transform hitPos;

    //Character components
    protected CapsuleCollider2D charCollider;
    protected Transform charTransform;

    //Defend
    private bool isDefending;

    //Death
    private bool dead;

    public float MaxHP { get; set; } = 100;
    public float CurrentHP { get; set; }

    protected virtual void Awake()
    {
        charCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Start()
    {
        CurrentHP = MaxHP;
    }

    protected virtual void Update()
    {
        AnimationManager();
        if (CurrentHP <= 0)
        {
            characterAnim.SetBool("Death", true);
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
            case State.Push:
                Push();
                break;
            default:
                break;
        }
    }

    private void AnimationManager()
    {
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
    }

    protected void ResetCharacter()
    {
        //Weapon
        rightWeapon.GetComponent<BoxCollider2D>().enabled = false;
        leftWeapon.GetComponent<BoxCollider2D>().enabled = false;

        rightWeapon.GetComponent<WeaponClass>().horizontalRecovery = 0.0f;
        rightWeapon.GetComponent<WeaponClass>().verticalRecovery = 0.0f;
        rightWeapon.GetComponent<WeaponClass>().damage = rightWeapon.GetComponent<WeaponClass>().baseDamage;
        rightWeapon.GetComponent<WeaponClass>().hit = true;
        rightWeapon.GetComponent<WeaponClass>().attacked = true;

        leftWeapon.GetComponent<WeaponClass>().specialRecovery = 0;
        leftWeapon.GetComponent<WeaponClass>().attacked = true;
        leftWeapon.GetComponent<WeaponClass>().hit = true;

        if (leftWeapon.name == "Shield")
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

        //Push
        //Enable collider
        charCollider.enabled = true;
        pushDistance = 0;
        //Finish movement
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
                    rightWeapon.GetComponent<WeaponClass>().HorizontalAttack();
                    break;

                case AttackState.Vertical:
                    //Animation
                    characterAnim.SetBool("VerticalAttack", true);
                    leftWeaponAnim.SetBool("VerticalAttack", true);
                    rightWeaponAnim.SetBool("VerticalAttack", true);
                    rightWeapon.GetComponent<WeaponClass>().VerticalAttack();
                    break;


                case AttackState.Special:
                    switch (leftWeapon.name)
                    {
                        case "Shield":
                            characterAnim.SetBool("ShieldAttack", true);
                            leftWeaponAnim.SetBool("ShieldAttack", true);
                            rightWeaponAnim.SetBool("ShieldAttack", true);
                            leftWeapon.GetComponent<WeaponClass>().SpecialAttack();
                            Dash();
                            break;
                        case "Net":
                            characterAnim.SetBool("NetAttack", true);
                            leftWeaponAnim.SetBool("NetAttack", true);
                            rightWeaponAnim.SetBool("NetAttack", true);
                            leftWeapon.GetComponent<WeaponClass>().SpecialAttack();
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
            charCollider.enabled = false;
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
            transform.position = Vector2.MoveTowards(transform.position, rollTarget, rollSpeed * Time.fixedDeltaTime);
        }

        if (rollRecovery <= 5.0f)
        {
            //Enable collider
            charCollider.enabled = true;
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
            state = State.Idle;
            stuned = true;
        }
    }

    internal void Push()
    {
        //Verifies if net hit something and if characterHit isnt null
        if (charCollider.enabled == true)
        {
            //Set collided character direction
            if (transform.position.x > hitPos.position.x)
            {
                //Set caracter target moving point
                characterTarget = new Vector2(transform.position.x + pushDistance, transform.position.y);
            }
            else
            {
                //Set caracter target moving point
                characterTarget = new Vector2(transform.position.x - pushDistance, transform.position.y);
            }
            //Disable collided character collider
            charCollider.enabled = false;

            pushRecovery = pushRecoveryTime;
        }
        //Verifies if characterHit isnt null and if action is in recovery
        else if (pushRecovery <= 0.1f)
        {
            state = State.Idle;
        }
        //Verifies if characterHit isnt null
        else
        {
            //Move collided character to target moving point
            transform.position = Vector2.MoveTowards(transform.position, characterTarget, pushSpeed * Time.fixedDeltaTime);
        }
    }

    internal void TakeDamage(float damage, float _pushDistance, Transform _hitPos)
    {
        if (isDefending)
        {
            characterAnim.SetTrigger("Block");
            leftWeaponAnim.SetTrigger("Block");
            rightWeaponAnim.SetTrigger("Block");
        }
        else if (_pushDistance != 0)
        {
            state = State.Push;
            hitPos = _hitPos;
            pushDistance = _pushDistance;
        }
        else
        {
            state = State.Stun;
        }
        CurrentHP -= damage;
        characterAnim.SetTrigger("Hit");
        leftWeaponAnim.SetTrigger("Hit");
        rightWeaponAnim.SetTrigger("Hit");
    }
}

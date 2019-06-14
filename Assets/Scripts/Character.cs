using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //ENUMS
    internal enum State { Idle, Roll, Climb, Attack, Run, Defend, Stun, Push, Dead, Clash }
    protected enum AttackState { Horizontal, Vertical, Special }

    internal State state;
    protected AttackState attackState;

    [Header("Animator")]
    [SerializeField] protected Animator characterAnim;
    [SerializeField] protected Animator leftWeaponAnim;
    [SerializeField] protected Animator rightWeaponAnim;

    [Header("Attack")]
    [SerializeField] protected float horizontalRecoveryTime;
    [SerializeField] protected float verticalRecoveryTime;
    [SerializeField] protected GameObject leftWeapon;
    [SerializeField] protected GameObject rightWeapon;

    protected float specialRecoveryTime;
    protected WeaponClass leftWeaponScript;
    protected WeaponClass rightWeaponScript;
    private BoxCollider2D leftWeaponCollider;
    private BoxCollider2D rightWeaponCollider;

    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;

    private Vector2 dashTarget;
    private bool dashed = true;

    [Header("Roll")]
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollRecoveryTime;
    [SerializeField] private float rollSpeed;

    private Vector2 rollTarget;
    private bool rolled = true;
    protected bool facingRight = true;

    [Header("Move")]
    [SerializeField] private Transform rightMovePoint;
    [SerializeField] private Transform leftMovePoint;
    [SerializeField] private float moveSpeed;

    private Vector2 rightMoveTarget;
    private Vector2 leftMoveTarget;

    [Header("Climb")]
    [SerializeField] private float climbDistance;
    [SerializeField] private float climbRecoveryTime;
    [SerializeField] protected float climbSpeed;

    private Vector2 climbTarget;
    private bool climbed = true;
    private bool atGround = true;

    [Header("Stun")]
    [SerializeField] private float stunRecoveryTime;

    [Header("Push")]
    [SerializeField] private float pushSpeed;
    [SerializeField] internal float pushRecoveryTime;

    private float pushDistance;
    Vector2 characterTarget;
    internal Transform hitPos;

    //Character components
    protected CapsuleCollider2D charCollider;
    protected Transform charTransform;

    //Defend
    private bool isDefending;

    //Recovery
    protected float recovery;
    private bool recovered;

    // SOUND *****
    [Header("SoundFX")]
    public AudioClip climbRoll;
    public AudioClip step;
    public AudioClip screamAtk;
    public AudioClip screamHit;
    public AudioClip screamStun;
    public AudioClip screamDeath;
    public AudioClip steps;
    public AudioClip shieldBlock;
    public AudioClip anyBlock; // Not implemented, missing condition

    // Properties
    public float MaxHP { get; set; }

    public float CurrentHP { get; set; }

    protected float zOffset = 0.0f;

    protected virtual void Awake()
    {
        characterAnim = gameObject.GetComponent<Animator>();
        charCollider = gameObject.GetComponent<CapsuleCollider2D>();

        rightWeaponAnim = rightWeapon.GetComponent<Animator>();
        leftWeaponAnim = leftWeapon.GetComponent<Animator>();

        rightWeaponScript = rightWeapon.GetComponent<WeaponClass>();
        leftWeaponScript = leftWeapon.GetComponent<WeaponClass>();

        rightWeaponCollider = rightWeapon.GetComponent<BoxCollider2D>();
        leftWeaponCollider = leftWeapon.GetComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
        //Attack
        horizontalRecoveryTime = 1.5f;
        verticalRecoveryTime = 1.5f;
        specialRecoveryTime = leftWeaponScript.specialRecoveryTime;

        //Dash
        dashDistance = 30.0f;
        dashSpeed = 30.0f;

        //Roll
        rollDistance = 60.0f;
        rollSpeed = 30.0f;
        rollRecoveryTime = 1.8f;

        //Move
        moveSpeed = 35.0f;

        //Climb
        climbDistance = 42.0f;
        climbSpeed = 20.0f;
        climbRecoveryTime = 2.0f;

        //Stun
        stunRecoveryTime = 1.0f;

        //Push
        pushRecoveryTime = 2.0f;
        pushSpeed = 50.0f;

        //HP
        MaxHP = 100.0f;
        CurrentHP = MaxHP;
    }

    protected virtual void Update()
    {
        AnimationManager();

        switch (state)
        {
            case State.Idle:
                ResetCharacter();
                break;
            case State.Stun:
                Recovery(stunRecoveryTime);
                break;
            case State.Roll:
                Roll();
                break;
            case State.Climb:
                Climb();
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

        Vector3 newPos = transform.position;
        newPos.z = newPos.y + 90.0f + zOffset;     
        transform.position = newPos;
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

    //Set all changed values during method execution to default
    protected void ResetCharacter()
    {
        //Weapon
        rightWeaponCollider.enabled = false;
        leftWeaponCollider.enabled = false;

        rightWeaponScript.damage = rightWeaponScript.baseDamage;
        rightWeaponScript.hit = true;
        rightWeaponScript.attacked = true;

        leftWeaponScript.attacked = true;
        leftWeaponScript.hit = true;

        //Dash
        dashed = true;

        //Roll
        rolled = true;

        //Climb
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
        charCollider.enabled = true;
        pushDistance = 0;

        //Recovery
        recovered = false;
    }

    protected void Attack()
    {
        switch (attackState)
        {
            case AttackState.Horizontal:
                characterAnim.SetBool("HorizontalAttack", true);
                leftWeaponAnim.SetBool("HorizontalAttack", true);
                rightWeaponAnim.SetBool("HorizontalAttack", true);
                rightWeaponScript.HorizontalAttack();
                Dash();
                Recovery(horizontalRecoveryTime);
                break;

            case AttackState.Vertical:
                characterAnim.SetBool("VerticalAttack", true);
                leftWeaponAnim.SetBool("VerticalAttack", true);
                rightWeaponAnim.SetBool("VerticalAttack", true);
                rightWeaponScript.VerticalAttack();
                Recovery(verticalRecoveryTime);
                break;

            case AttackState.Special:
                switch (leftWeapon.name)
                {
                    case "Shield":
                        characterAnim.SetBool("ShieldAttack", true);
                        leftWeaponAnim.SetBool("ShieldAttack", true);
                        rightWeaponAnim.SetBool("ShieldAttack", true);
                        leftWeaponScript.SpecialAttack();
                        Dash();
                        break;
                    case "Net":
                        characterAnim.SetBool("NetAttack", true);
                        leftWeaponAnim.SetBool("NetAttack", true);
                        rightWeaponAnim.SetBool("NetAttack", true);
                        leftWeaponScript.SpecialAttack();
                        break;
                    default:
                        break;
                }
                Recovery(specialRecoveryTime);
                break;
            default:
                break;
        }
    }

    private void Dash()
    {
        if (dashed)
        {
            //Set dash direction
            if (transform.right.x > 0.0f)
            {
                //Dash right
                dashTarget = new Vector2(transform.position.x + dashDistance, transform.position.y);
            }
            else
            {
                //Dash left
                dashTarget = new Vector2(transform.position.x - dashDistance, transform.position.y);
            }
            //Confirm that character dashed
            dashed = false;
            //Enable weapon collider
            if (attackState == AttackState.Horizontal)
            {
                rightWeaponCollider.enabled = true;
            }
            else if (attackState == AttackState.Special)
            {
                leftWeaponCollider.enabled = true;
            }
        }
        else if (transform.position.x != dashTarget.x)
        {
            //Move character
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.fixedDeltaTime);
        }
    }

    private void Roll()
    {
        if (rolled)
        {
            //Disable collider
            charCollider.enabled = false;
            //Animation
            characterAnim.SetTrigger("Roll");
            leftWeaponAnim.SetTrigger("Roll");
            rightWeaponAnim.SetTrigger("Roll");

            // Sound
            SoundManager.PlaySound(climbRoll, 0.20f, Random.Range(1.0f, 3.0f));

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
            //Confirm that character rolled
            rolled = false;
        }
        else if (transform.position.x != rollTarget.x)
        {
            //Move character
            transform.position = Vector2.MoveTowards(transform.position, rollTarget, rollSpeed * Time.fixedDeltaTime);
        }

        Recovery(rollRecoveryTime);
    }

    private void Climb()
    {
        if (climbed)
        {
            //Disable collider
            charCollider.enabled = false;
            atGround = !atGround;
            //Animation
            characterAnim.SetBool("Climb", true);
            leftWeaponAnim.SetBool("Climb", true);
            rightWeaponAnim.SetBool("Climb", true);

            // Sound, adjustable volume and "random" pitch
            SoundManager.PlaySound(climbRoll, 0.20f, Random.Range(4.5f, 6.0f));

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
            //Confirm that character climbed
            climbed = false;
        }
        else if (transform.position.y != climbTarget.y)
        {
            //Move character
            transform.position = Vector2.MoveTowards(transform.position, climbTarget, climbSpeed * Time.fixedDeltaTime);
        }

        Recovery(climbRecoveryTime);
    }

    private void MoveRight()
    {
        //Animation
        characterAnim.SetBool("Run", true);
        leftWeaponAnim.SetBool("Run", true);
        rightWeaponAnim.SetBool("Run", true);
        //Set character movement direction

        // Sound
        //SoundManager.PlaySound(steps, 0.1f, Random.Range(1.0f, 1.5f)); 

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

    protected void Recovery(float recoveryTime)
    {
        //Discount recovery time
        recovery -= Time.fixedDeltaTime;

        if (!recovered)
        {
            recovery = recoveryTime;
            recovered = true;
        }

        if (recovery <= 0.0f)
        {
            state = State.Idle;
            ResetCharacter();
            recovered = false;
        }
    }

    protected void Push()
    {
        //Verifies if character collider is active
        if (charCollider.enabled == true)
        {
            //Set character direction
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
            //Disable character collider
            charCollider.enabled = false;
        }
        else
        {
            //Move character to target moving point
            transform.position = Vector2.MoveTowards(transform.position, characterTarget, pushSpeed * Time.fixedDeltaTime);
        }
        Recovery(pushRecoveryTime);
    }

    protected virtual void Death()
    {
        characterAnim.SetBool("Death", true);
        leftWeaponAnim.SetBool("Death", true);
        rightWeaponAnim.SetBool("Death", true);

        // Sound
        SoundManager.PlaySound(screamDeath, 10.0f, Random.Range(1.0f, 2.0f));                                                                                          /////////////   

        charCollider.enabled = false;
        this.enabled = false;

        zOffset += 2.0f;
        Vector3 newPos = transform.position;
        newPos.z = newPos.y + 90.0f + zOffset;
        transform.position = newPos;
    }

    internal void TakeDamage(float damage, float _pushDistance, Transform _hitPos)
    {
        if (isDefending && _pushDistance == 0)
        {
            characterAnim.SetTrigger("Block");
            leftWeaponAnim.SetTrigger("Block");
            rightWeaponAnim.SetTrigger("Block");

            // Shield block sound
            if (leftWeapon.name == "Shield")
            {
                SoundManager.PlaySound(shieldBlock, 0.2f, Random.Range(1.0f, 1.5f));
            }
            // If no shield in hand
            else
            {
                SoundManager.PlaySound
                (anyBlock, Random.Range(5.0f, 7.0f), Random.Range(6.0f, 7.0f));
            }
        }
        else if (_pushDistance != 0)
        {
            ResetCharacter();
            state = State.Push;
            hitPos = _hitPos;
            pushDistance = _pushDistance;

            CurrentHP -= damage;
            characterAnim.SetTrigger("Hit");
            leftWeaponAnim.SetTrigger("Hit");
            rightWeaponAnim.SetTrigger("Hit");

            // Hurt
            SoundManager.PlaySound(screamHit, 2.0f, Random.Range(1.0f, 1.1f));
        }
        else
        {
            ResetCharacter();
            state = State.Stun;

            CurrentHP -= damage;
            characterAnim.SetTrigger("Hit");
            leftWeaponAnim.SetTrigger("Hit");
            rightWeaponAnim.SetTrigger("Hit");

            // Hurt stun Sound
            SoundManager.PlaySound(screamStun, 2.0f, Random.Range(1.0f, 1.1f));
        }
    }
}
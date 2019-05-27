using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    [Header("Enemy")]
    //Animator
    private Animator playerAnim;
    [SerializeField] private Animator weaponRightAnim;
    [SerializeField] private Animator weaponLeftAnim;
    //AI
    private AnimatorStateInfo playerState;
    //
    int randomAttack;
    //Run
    [SerializeField] private float playerDistance = 5f;
    private Transform playerTransform;
    private bool onRight = true;
    private bool saveOnRight = false;
    //MoveTowards targets
    Vector2 runTarget;
    Vector2 climbTarget;
    Vector2 rollTarget;
    //Roll
    [SerializeField] private float rollRange = 30;
    private bool didRoll;
    //Stats
    private int agro, def, agil;
    private int currentAgro, currentDef, currentAgil;

    protected override void Start()
    {
        base.Start();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        agro = Random.Range(4, 10);
        def = Random.Range(1, 10);
        agil = Random.Range(1, 10); ;

        StartCoroutine(SetState());
        CurrentHP = 250;

    }
    private void FixedUpdate()
    {
        runTarget = new Vector2(playerTransform.position.x, transform.position.y);
        climbTarget = new Vector2(transform.position.x, playerTransform.position.y);
    }
    void Update()
    {
        //On death
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
        }

        UpdateState();
        switch (state)
        {
            case State.Attack:
                randomAttack = Random.Range(1, 4);
                AttackState();
                state = State.Idle;
                break;
            case State.Run:
                RunState();
                break;
            case State.Defend:
                DefendState();
                state = State.Idle;
                break;
            case State.Roll:
                RollState();
                didRoll = false;
                break;
            case State.Climb:
                ClimbState();
                break;
            default:
                break;
        }
    }

    //AI Methods
    private void UpdateState()
    {
        playerState = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (playerState.IsName("Attack"))
        {
            currentAgil = Random.Range(1, 10) + agil;
            currentAgro = Random.Range(1, 5) + agro;
            currentDef = Random.Range(1, 10) + def;
        }
        else if (playerState.IsName("Defend"))
        {
            currentAgil = Random.Range(1, 10) + agil;
            currentAgro = Random.Range(1, 10) + agro;
            currentDef = Random.Range(1, 5) + def;
        }
        else if (playerState.IsName("Roll"))
        {
            currentAgil = Random.Range(1, 5) + agil;
            currentAgro = Random.Range(1, 10) + agro;
            currentDef = Random.Range(1, 10) + def;
        }
        else
        {
            currentAgil = Random.Range(1, 10) + agil;
            currentAgro = Random.Range(1, 10) + agro;
            currentDef = Random.Range(1, 10) + def;
        }
    }

    private IEnumerator SetState()
    {
        while (true)
        {
            //Run
            if (Vector2.Distance(transform.position, playerTransform.position) > playerDistance
                && transform.position.y == playerTransform.position.y)
            {
                state = State.Run;
            }
            else if ((transform.position.y != playerTransform.position.y))
            {
                state = State.Climb;
            }
            else if (currentAgil > currentAgro && currentAgil > currentDef)
            {
                state = State.Roll;
                didRoll = true;
                if (playerTransform.position.x > transform.position.x)
                {
                    rollTarget = new Vector2(transform.position.x + rollRange, transform.position.y);
                }
                else
                {
                    rollTarget = new Vector2(transform.position.x - rollRange, transform.position.y);
                }
            }
            else if (currentAgro > currentAgil && currentAgro > currentDef)
            {
                state = State.Attack;
            }
            else if (currentDef > currentAgil && currentDef > currentAgro)
            {
                state = State.Defend; ;
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    //Action Methods
    private void AttackState()
    {


        if (randomAttack == 1)
        {
            switch (leftWeapon.name)
            {
                case "Net":
                    animator.SetTrigger("NetAttack");
                    weaponLeftAnim.SetTrigger("NetAttack");
                    weaponRightAnim.SetTrigger("NetAttack");
                  //  rightWeapon.GetComponent<Net>().Attack();
                    break;
                case "Shield":
                    animator.SetTrigger("ShieldAttack");
                    weaponLeftAnim.SetTrigger("ShieldAttack");
                    weaponRightAnim.SetTrigger("ShieldAttack");
                  //  rightWeapon.GetComponent<Shield>().Attack();
                    break;
                default:
                    break;
            }
            randomAttack = 0;
        }
        else if (randomAttack == 2)
        {
            animator.SetTrigger("HorizontalAttack");
            weaponRightAnim.SetTrigger("HorizontalAttack");
            weaponLeftAnim.SetTrigger("HorizontalAttack");
            switch (rightWeapon.name)
            {
                case "Trident":
                 //   rightWeapon.GetComponent<Trident>().Attack();
                    break;
                case "LongSwordRight":
                  //  rightWeapon.GetComponent<LongSword>().Attack();
                    break;
                default:
                    break;
            }
            randomAttack = 0;
        }
        else if (randomAttack == 3)
        {
            animator.SetTrigger("VerticalAttack");
            weaponRightAnim.SetTrigger("VerticalAttack");
            weaponLeftAnim.SetTrigger("HorizontalAttack");
            switch (rightWeapon.name)
            {
                case "Trident":
                   // rightWeapon.GetComponent<Trident>().Attack();
                    break;
                case "LongSwordRight":
                   // rightWeapon.GetComponent<LongSword>().Attack();
                    break;
                default:
                    break;
            }
            randomAttack = 0;
        }
    }

    private void DefendState()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < playerDistance)
        {
            animator.SetBool("Defense", true);
            weaponLeftAnim.SetBool("Defense", true);
            weaponRightAnim.SetBool("Defense", true);
        }
        else
        {
            animator.SetBool("Defense", false);
            weaponLeftAnim.SetBool("Defense", false);
            weaponRightAnim.SetBool("Defense", false);
        }

    }

    private void RunState()
    {
        if (Vector2.Distance(transform.position, runTarget) > playerDistance
            && animator.GetBool("Defense") == false)
        {
            animator.SetBool("Run", true);
            weaponLeftAnim.SetBool("Run", true);
            weaponRightAnim.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(transform.position, runTarget, walkSpeed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetBool("Run", false);
            weaponLeftAnim.SetBool("Run", false);
            weaponRightAnim.SetBool("Run", false);
        }

        onRight = ((playerTransform.position.x - transform.position.x) < 0);
        if (onRight != saveOnRight)
        {
            RotateCharacter();
            saveOnRight = onRight;
        }
    }

    private void ClimbState()
    {
        if (transform.position.y != playerTransform.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, climbTarget, climbSpeed * Time.fixedDeltaTime);
            animator.SetBool("Climb", true);
            weaponLeftAnim.SetBool("Climb", true);
            weaponRightAnim.SetBool("Climb", true);
        }
        else
        {
            animator.SetBool("Climb", false);
            weaponLeftAnim.SetBool("Climb", false);
            weaponRightAnim.SetBool("Climb", false);
        }
    }

    private void RollState()
    {
        if (transform.position.x != rollTarget.x)
        {
            if (didRoll)
            {
                animator.SetTrigger("Roll");
                weaponRightAnim.SetTrigger("Roll");
                weaponLeftAnim.SetTrigger("Roll");
            }
            transform.position = Vector2.MoveTowards(transform.position, rollTarget, maxRollSpeed * Time.fixedDeltaTime);
        }
    }
}
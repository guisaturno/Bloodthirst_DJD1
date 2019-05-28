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
    //Attack
    int randomAttack;

    //Player position
    [SerializeField] private float playerDistance = 5f;
    private Transform playerTransform;

    //MoveTowards targets
    //Vector2 climbTarget;

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
    protected override void Update()
    {
        base.Update();

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
            case State.Defend:
                DefendState();
                state = State.Idle;
                break;
            case State.Climb:
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
            if (Vector2.Distance(transform.position, playerTransform.position) > playerDistance
                && transform.position.y == playerTransform.position.y)
            {
                if (transform.position.x < playerTransform.position.x)
                {
                    transform.rotation = Quaternion.identity;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                state = State.Run;
            }
            else if ((transform.position.y != playerTransform.position.y))
            {
                state = State.Climb;
            }
            else if (currentAgil > currentAgro && currentAgil > currentDef)
            {
                state = State.Roll;
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
}
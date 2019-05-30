using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    [Header("Enemy")]
    //AI
    private Animator playerAnim;
    private AnimatorStateInfo playerState;
    //Attack
    int randomAttack;

    //Player position
    [SerializeField] private float playerDistance = 5f;
    private Transform playerTransform;

    //Stats
    private int agro, def, agil;
    private int currentAgro, currentDef, currentAgil;

    protected override void Start()
    {
        base.Start();

        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        agro = Random.Range(4, 10);
        def = Random.Range(1, 6);
        agil = Random.Range(1, 6); ;

        StartCoroutine(SetState());
        CurrentHP = 250;

    }
    protected override void Update()
    {
        base.Update();
        UpdateState();
        if (Vector2.Distance(transform.position, playerTransform.position) < playerDistance && state == State.Run)
        {
            state = State.Idle;
        }
        //On death
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
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
                randomAttack = Random.Range(1, 4);
                SelectAttack();
                state = State.Attack;
            }
            else if (currentDef > currentAgil && currentDef > currentAgro)
            {
                state = State.Defend; ;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
    private void SelectAttack()
    {
        if (randomAttack == 1)
        {
            attackState = AttackState.Special;
        }
        else if (randomAttack == 2)
        {
            attackState = AttackState.Horizontal;
        }
        else if (randomAttack == 3)
        {
            attackState = AttackState.Vertical;
        }
    }
}
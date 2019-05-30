using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    [Header("Enemy")]
    //AI
    private Animator playerAnim;
    private AnimatorStateInfo playerState;

    //Player position
    [SerializeField] private float playerDistance = 25f;
    private Transform playerTransform;

    //Stats
    private int agro, def;
    private int currentAgro, currentDef;

    protected override void Start()
    {
        base.Start();

        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        agro = Random.Range(4, 10);
        def = Random.Range(1, 6);

        StartCoroutine(SetState());
    }

    protected override void Update()
    {
        base.Update();

        UpdateState();

        if (transform.position.x < playerTransform.position.x)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        if (Vector2.Distance(transform.position, playerTransform.position) < playerDistance && state == State.Run)
        {
            state = State.Idle;
        }
    }

    //AI Methods
    private void UpdateState()
    {
        playerState = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (playerState.IsName("Attack"))
        {
            currentAgro = Random.Range(1, 10) + agro;
            currentDef = Random.Range(5, 10) + def;
        }
        else if (playerState.IsName("Defend"))
        {
            currentAgro = Random.Range(5, 10) + agro;
            currentDef = Random.Range(1, 10) + def;
        }
        else if (playerState.IsName("Roll"))
        {
            currentAgro = Random.Range(5, 10) + agro;
            currentDef = Random.Range(1, 5) + def;
        }
        else
        {
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
                state = State.Run;
            }
            else if ((transform.position.y != playerTransform.position.y))
            {
                state = State.Climb;
            }
            else if (Vector2.Distance(transform.position, playerTransform.position) <= 15)
            {
                state = State.Roll;
            }
            else if (currentAgro > currentDef)
            {
                SelectAttack();
                state = State.Attack;
            }
            else if (currentDef > currentAgro)
            {
                state = State.Defend; ;
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    private void SelectAttack()
    {

        if (Vector2.Distance(transform.position, playerTransform.position) <= 20)
        {
            attackState = AttackState.Special;
        }
        else if (Vector2.Distance(transform.position, playerTransform.position) <= 30)
        {
            attackState = AttackState.Vertical;
        }
        else
        {
            attackState = AttackState.Horizontal;
        }
    }
}
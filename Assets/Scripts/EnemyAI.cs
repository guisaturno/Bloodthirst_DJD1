using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    [Header("Enemy")]
    //Spawn control
    private RoundManager roundManager;
    //AI
    private Animator playerAnim;
    private AnimatorStateInfo playerState;
    private float responseTime;

    //Player position
    [SerializeField] private float playerDistance = 25f;
    private Transform playerTransform;

    //Stats
    private int agro, def;
    private int currentAgro, currentDef;

    protected override void Awake()
    {
        base.Awake();

        zOffset = 5f;
    }

    protected override void Start()
    {
        base.Start();

        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        agro = Random.Range(4, 10);
        def = Random.Range(1, 6);

        StartCoroutine(SetState());
        StartCoroutine(Rotate());
    }

    protected override void Update()
    {
        if (CurrentHP <= 0)
        {
            Death();
        }

        

        if (PauseMenu.pauseGame == false)
        {
            base.Update();
            UpdateState();

            if (Vector2.Distance(transform.position, playerTransform.position) < playerDistance && state == State.Run)
            {
                state = State.Idle;
            }
        }
        else
        {
            ResetCharacter();
            AnimationManager();
            state = State.Idle;
        }
    }

    public void NewEnemy(RoundManager _roundManager, float _responseTime)
    {
        roundManager = _roundManager;
        responseTime = _responseTime;
    }

    private IEnumerator SetState()
    {
        while (true)
        {
            if (recovery > 0.0f)
            {
                yield return new WaitForSeconds(recovery);
            }
            else if (Vector2.Distance(transform.position, playerTransform.position) > playerDistance
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
                state = State.Defend;
            }

            yield return new WaitForSeconds(responseTime);
        }
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            if (transform.position.x < playerTransform.position.x && state != State.Roll
           && transform.position.x < playerTransform.position.x && state != State.Attack)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (state != State.Roll && state != State.Attack)
            {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            yield return new WaitForSeconds(.5f);
        }
    }

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

    private void SelectAttack()
    {

        if (Vector2.Distance(transform.position, playerTransform.position) <= 25)
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

    protected override void Death()
    {
        StopAllCoroutines();
        roundManager.enemysAlive -= 1;
        base.Death();
    }

}
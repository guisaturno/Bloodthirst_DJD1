﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : Character
{
    private bool isCoroutineExecuting = false;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (state != State.Stun && PauseMenu.pauseGame == false)
        {
            if (state == State.Idle || state == State.Run)
            {
                //Vertical attack
                if (Input.GetKeyDown(KeyCode.X))
                {
                    state = State.Attack;
                    attackState = AttackState.Vertical;
                }
                //Horizontal attack
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    state = State.Attack;
                    attackState = AttackState.Horizontal;
                }
                //Special attack
                if (Input.GetKeyDown(KeyCode.V))
                {
                    state = State.Attack;
                    attackState = AttackState.Special;
                }
                // Roll - player
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    state = State.Roll;
                }
                // Climb - player
                if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    state = State.Climb;
                }
                //Defense
                if (Input.GetKey(KeyCode.C))
                {
                    state = State.Defend;
                }
            }
            //Run
            if (Input.GetKey(KeyCode.RightArrow) && state == State.Idle)
            {
                //Set character facing direction
                transform.rotation = Quaternion.identity;
                state = State.Run;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && state == State.Idle)
            {
                //Set character facing direction
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                state = State.Run;
            }
            //Set state back to idle when stop running
            if (Input.GetKeyUp(KeyCode.LeftArrow) && state == State.Run
            || Input.GetKeyUp(KeyCode.RightArrow) && state == State.Run)
            {
                state = State.Idle;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                state = State.Idle;
            }
        }

        //On death
        if (CurrentHP <= 0)
        {
            state = State.Dead;
            Death();
            StartCoroutine(ExecuteAfterTime(4f));
        }
        //Restart scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator ExecuteAfterTime(float delay)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(delay);

        // Code to execute after the delay
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        isCoroutineExecuting = false;
    }
}

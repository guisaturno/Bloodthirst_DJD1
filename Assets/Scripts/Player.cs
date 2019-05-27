using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    [Header("Player")]
    private Collider2D playerCollider;
    private WeaponClass weapon;

    // Methods
    protected override void Start()
    {
        // Call parent method
        base.Start();
        playerCollider = GetComponent<Collider2D>();
        weapon = GetComponent<WeaponClass>();
    }

    protected override void Update()
    {
        //Set animator booleans to false according to state
        base.Update();

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Set character facing direction
            transform.rotation = Quaternion.identity;
            state = State.Run;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Set character facing direction
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            state = State.Run;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            state = State.Idle;
        }

        //Defense
        if (Input.GetKey(KeyCode.C))
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
        //On death
        if (CurrentHP <= 0)
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        // Attack
        if (timeAttacking <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LeftWeaponAttack();
                timeAttacking = startTimeAttack;
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                weapon.HorizontalAttack = true;
                RightWeaponAttack();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                weapon.HorizontalAttack = false;
                RightWeaponAttack();
            }
        }
        else
        {
            timeAttacking -= Time.deltaTime;
        }
    }
}

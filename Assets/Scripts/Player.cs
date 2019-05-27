using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    [Header("Player")]
    private float xAxis;
    private float yAxis;
    private float saveAxis = 1f;

    private Collider2D playerCollider;
    private WeaponClass weapon;

    // Methods
    protected override void Start()
    {
        // Call parent method
        base.Start();
        state = State.Run;
        playerCollider = GetComponent<Collider2D>();
        weapon = GetComponent<WeaponClass>();
    }


    private void FixedUpdate()
    {
        // Local variables
        currentVelocity = rb.velocity;

        switch (state) {
            case State.Run:
                currentVelocity = new Vector2(xAxis * walkSpeed * Time.fixedDeltaTime, currentVelocity.y);
                break;

            case State.Roll:
                RollCharacter();
                break;

            case State.Climb:
                ClimbCharacter();
                break;
        }

        rb.velocity = currentVelocity;
    }

    private void Update()
    {
        // Mechanic input
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

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
        if (timeRolling <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                state = State.Roll;
                rollSpeed = maxRollSpeed;
                timeRolling = startTimeRolling;
            }
        }
        else
        {
            timeRolling -= Time.deltaTime;
        }

        // Climb - player
        if(timeClimbing <= 0)
        {
            if (canClimb)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    state = State.Climb;
                    timeClimbing = startTimeClimbing;
                    canClimb = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    state = State.Climb;
                    timeClimbing = startTimeClimbing;
                    canClimb = true;
                }
            }
        }
        else
        {
            timeClimbing -= Time.deltaTime;
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

        // Flip - player
        if (xAxis != 0)  // key pressed
        {
            if (xAxis != saveAxis) 
                RotateCharacter();

            saveAxis = xAxis;
        }

        rb.velocity = currentVelocity;
    }
}

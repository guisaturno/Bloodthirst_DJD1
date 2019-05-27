using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Run
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
        //Defense
        if (Input.GetKey(KeyCode.C))
        {
            anim.SetBool("Defense", true);
            if (Input.GetKeyDown(KeyCode.B))
            {
                anim.SetTrigger("Block");
            }
        }
        else
        {
            anim.SetBool("Defense", false);
        }
        //Roll
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetTrigger("Roll");
        }
        //Horizontal attack
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.RightArrow) 
            || Input.GetKeyDown(KeyCode.X) &&  Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetTrigger("HorizontalAttack");
        }
        //Vertical attack
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("VerticalAttack");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetTrigger("ShieldAttack");
        }
        //Pick weapon
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("PickWeapon");
        }
        //Hit
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("Parry");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("Death");
        }

    }
}

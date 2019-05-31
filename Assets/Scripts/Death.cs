using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Animator leftWeapon;
    [SerializeField] private Animator rightWeapon;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Death", true);
        leftWeapon.SetBool("Death", true);
        rightWeapon.SetBool("Death", true);
    }
}

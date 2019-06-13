using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRunning : MonoBehaviour
{
    //Variables
    [SerializeField]private Character character;
    private CircleCollider2D myCollider;
    private void Start()
    {
        myCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.CompareTag("Player") && character.state == Character.State.Run
        || obj.CompareTag("Enemy") && character.state == Character.State.Run)
        {
            character.state = Character.State.Idle;
            print("Stop running");
        }
    }
}

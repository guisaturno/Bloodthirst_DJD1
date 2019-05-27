using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            collision.GetComponent<Player>().CurrentHP -= 20;
        }
        else if (collision.name == "Enemy")
        {
            collision.GetComponent<EnemyAI>().CurrentHP -= 20;
        }
    }
}

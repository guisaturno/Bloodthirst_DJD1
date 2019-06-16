using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    [SerializeField] private Text roundText;
    public static int enemysKilled;

    void Start()
    {
        enemysKilled = 0;
    }

    void Update()
    {
        roundText.text = "Kills " + enemysKilled;
    }
}

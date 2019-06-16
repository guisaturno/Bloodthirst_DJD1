using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCount : MonoBehaviour
{
    [SerializeField] private Text roundText;
    public static int rounds;

    void Start()
    {
        rounds = 0;
    }

    void Update()
    {
        roundText.text = "Round " + rounds;
    }
}

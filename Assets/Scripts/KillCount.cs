using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    [SerializeField] private Text killText;
    public static float enemiesKilled;

    void Start()
    {
        enemiesKilled = 0f;
    }

    void Update()
    {
        if(enemiesKilled > PlayerPrefs.GetFloat("MostKills"))
            PlayerPrefs.SetFloat("MostKills", enemiesKilled);

        killText.text = "Kills: " + Mathf.Round(enemiesKilled) + "\n Most Kills: " + PlayerPrefs.GetFloat("MostKills");
    }
}

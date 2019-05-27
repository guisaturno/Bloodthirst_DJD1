using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        StartScene();
        Quit();
    }

    public void StartScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        { 
            Application.Quit();
        }
    }
}

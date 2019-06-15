using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Animator transition;
    private bool changedScene = false;

    private void Update()
    {
        StartScene();
        Quit();
    }

    public void StartScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadGame());
        }
    }

    IEnumerator LoadGame()
    {
        if (!changedScene)
        {
            transition.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Main");
            changedScene = true;
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

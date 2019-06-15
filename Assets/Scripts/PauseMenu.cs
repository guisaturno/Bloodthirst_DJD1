using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame { get; private set; } = true;
    [SerializeField] private GameObject pauseMenu;
    private GameObject selected;

    private void Start()
    {
        selected = GameObject.Find("Start");
        Time.timeScale = 0f;
    }

    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                PauseGame = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                PauseGame = true;
            }
        }

        if (selected)
        {
            EventSystem.current.firstSelectedGameObject = selected;
        }

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            CatchClicks(selected);
        }
    }

    public void StartGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToControls()
    {

    }

    public void SelectGameMode()
    {

    }

    public void Exit()
    {
        Debug.Log("QUIT");
        Application.Quit(0);
    }

    public void CatchClicks(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(selected);
    }
}

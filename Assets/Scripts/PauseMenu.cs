using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool pauseGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject controlsImage;
    [SerializeField] private GameObject controlsText;
    private bool isPressed;

    private GameObject selected;

    private void Start()
    {
        selected = GameObject.Find("Start");
        pauseGame = true;
        isPressed = false;
    }

    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseGame)
            {
                pauseMenu.SetActive(false);
                pauseGame = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                pauseGame = true;
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
        pauseGame = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowControls()
    {
        isPressed = !isPressed;
        controlsImage.SetActive(isPressed);
        controlsText.SetActive(isPressed);
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

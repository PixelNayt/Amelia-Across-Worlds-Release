using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Script for the "MainGame" scene
    //Attach to an Object called "GameManger" in the Hierarchy

    //References to Pause Menu and Options Screen
    public GameObject pauseMenu;
    public GameObject optionsScreen;

    //For future reference: Add a bool check to areas of code with input eg. player attack etc.
    public static bool isPaused;

    void Start()
    {
        isPaused = false;
    }

    void Update()
    {

        //If player presses Esc. pause the game or unpause the 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            } 
            else
            {
                PauseGame();
            }
        }

    }

    //PAUSE & RESUME
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        //Called when player clicks the Resume Button, is attached to the 
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    //MAIN MENU
    //Return time to normal and go to the Main Menu scene, turn isPaused to false
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }

    //OPEN & CLOSE OPTIONS
    //Sets the Options Screen as Active or Inactive in the Unity Editor Hierarchy
    //Attach to respective buttons
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

}

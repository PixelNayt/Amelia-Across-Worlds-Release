using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Reference to the various Gameobject(s) in Hierarchy
    public GameObject optionsScreen;
    public GameObject creditsScreen;
    public GameObject tutorial;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //PLAY
    public void StartGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    //When clicked, the Object is either Hidden or Shown / Inactive or Active
    //Attach these functions to the specified buttons

    //OPTIONS
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }


    //CREDITS
    public void OpenCredits()
    {
        creditsScreen.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
    }


    //TUTORIAL
    public void openTutorial()
    {
        tutorial.SetActive(true);
    }

    public void closeTutorial()
    {
        tutorial.SetActive(false);
    }


    //QUIT GAME
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitted the Game");
    }
}

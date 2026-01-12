using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] //Adds or shows the the image below to the unity inspector
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
        //Start the async operation
        //uses a coroutine, to constantly check the progress of loading the game scene then we can take the progress
        //and assign it to the fill amount

        StartCoroutine(loadASyncOperation());

    }

    IEnumerator loadASyncOperation()
    {
        //ASync operation here basically takes a scene and begins loading it in the background of another scene
        //Async operations allow the application to multi-task in a practical sense by loading another scene in a different scene

        //Create an async  operation
        AsyncOperation loadingProgress = SceneManager.LoadSceneAsync("MainGame");

        while(loadingProgress.progress < 1)
        {
            //The fill of the progress bar = async operation progress
            progressBar.fillAmount = loadingProgress.progress;
            yield return new WaitForEndOfFrame();
        }

        //when done or the scene is loaded, it loads the "MainGame" scene and leaves the "LoadingScene"

    }

}

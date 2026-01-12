using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    //References
    [SerializeField]
    public Text countdownText;

    public GameObject winGame;

    //Variables
    float currentTime = 0f;
    float startingTime = 300f; //300 seconds = survive 5 mins to win

    void Start()
    {
        currentTime = startingTime;

        //Since the timeScale value from the "Win the Game" carries between set it to 1 again every time we start this script to prevent pausing
        Time.timeScale = 1;
    }

    void Update()
    {
        //Decrease currentTime by 1 every second by multiplying by DeltaTime
        currentTime -= 1 * Time.deltaTime;

        //Access the text of the "Countdown Text" Object and change it's values according to the current time
        //Also convert the value of currentTime to a string because text only uses strings
        countdownText.text = currentTime.ToString("0");

        //Prevent the system from counting down to negative numbers and set it only to 0
        if(currentTime <= 0)
        {
            currentTime = 0;

            //Win the game!
            //Then also go back to the Main Menu wwww ssg cheap game design
            Time.timeScale = 0;
            winGame.SetActive(true);
        }
    }
}

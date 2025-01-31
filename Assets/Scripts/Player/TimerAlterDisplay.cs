using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerAlterDisplay : MonoBehaviour
{
    public float timeRemaining = 10f;
    public float endTime = 0f;
    public bool timerRunning = false;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerRunning = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > endTime)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timerRunning = false;
                timeRemaining = 0;
                timerText.text = string.Format("00:00");
                SceneManager.LoadScene("Lose");
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); //2
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); //10
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

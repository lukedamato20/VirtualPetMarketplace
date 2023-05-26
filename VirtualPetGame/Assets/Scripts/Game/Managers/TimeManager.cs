using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float gameTime, hourLengthMultiplier;
    public static float gameHourTimer;
    public static bool GameIsPaused = false;

    void Start()
    {
        Time.timeScale = hourLengthMultiplier;

    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameHourTimer <= 0)
        {
            gameHourTimer = hourLengthMultiplier;
        }
        else
        {
            gameHourTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resumed");
        Time.timeScale = hourLengthMultiplier;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Debug.Log("Paused");
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


}

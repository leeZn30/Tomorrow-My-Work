using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Slider slTimer;
    float StageTime;

    private bool isTimeOut = false;
    private bool isRunning = true;

    void Start()
    {
        slTimer = GetComponent<Slider>();
        StageTime = GameManager.Instance.StageTime;
        slTimer.maxValue = StageTime;
        slTimer.value = StageTime;
    }

    void Update()
    {
        if (!isTimeOut)
        {
            TimeSpend();
        }
        else if (isTimeOut && !isRunning)
        {
            GameManager.Instance.GameOver();
            isRunning = true;
        }
    }

    void TimeSpend()
    {
        if (slTimer.value > 0.0f)
        {
            slTimer.value -= Time.deltaTime;
        }
        else
        {
            isTimeOut = true;
            isRunning = false;
        }
    }

}

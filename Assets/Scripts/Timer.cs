using UnityEngine;
using System.Collections;


public class Timer : MonoBehaviour 
{

    public float currentTime;
    public bool isFinished = false;
    public bool isRunning = false;
    private bool countUp = false;
 

	// Use this for initialization
	void Start () 
    {
	
	}


    /// <summary>
    /// Reset the timer.
    /// </summary>
    public void Reset()
    {
        isFinished = false;
        isRunning = false;
        currentTime = 0;
    }


    /// <summary>
    /// Countdown timer.
    /// </summary>
    /// <param name='countdownTime'>
    /// Time to countdown from.
    /// </param>
    public void Countdown(float countdownTime)
    {
        currentTime = countdownTime;
        isFinished = false;
        isRunning = true;
        countUp = false;
    }

    public void LapTimer()
    {
        currentTime = 0;
        isFinished = false;
        isRunning = true;
        countUp = true;
    }

    /// <summary>
    /// Update the timer.
    /// </summary>
    /// <returns>
    /// If the timer is finished (true).
    /// </returns>
	// Update is called once per frame
	void Update () 
    {
        if (isRunning)
        {
            if (countUp)
                currentTime += Time.deltaTime;
            else
                currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                isRunning = false;
                isFinished = true;
            }
        }
	}
}

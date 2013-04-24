using UnityEngine;
using System.Collections;

public class lapController : MonoBehaviour 
{

    //timer that will control lap
    public Timer lapTimer;
	public GUIText currentLapText;
	public GUIText fastestLapText;
    public float fastestTime = 0;

	// Use this for initialization
	void Start () 
    {
        lapTimer.LapTimer();
		fastestLapText.text = "";
	}
	
	void Update()
	{
		currentLapText.text = "Current Lap: " + lapTimer.currentTime;	
	}

    void OnTriggerEnter(Collider collider)
    {
		//checking to see when the ship crosses the finish line
        if (collider.name == "ShipContainer" && transform.InverseTransformDirection( collider.attachedRigidbody.velocity ).z < 0)
		{
			//sets your first lap as the fastest
			if(fastestTime == 0)
			{
				fastestTime = lapTimer.currentTime;
			}
			//if your current lap is less then your fastest, make your current lap the new fastest
			else if(fastestTime > lapTimer.currentTime)
			{
				fastestTime = lapTimer.currentTime;	
			}
			
			fastestLapText.text = "Fastest Lap: " + fastestTime.ToString();
			lapTimer.Reset();
            lapTimer.LapTimer();
		}
	}
}

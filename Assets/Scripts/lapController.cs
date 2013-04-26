using UnityEngine;
using System.Collections;

/// <summary>
/// Lap controller.
/// </summary>
/// <author>Pete O'Neal</author>
/// <author>Daniel Jost</author>
public class lapController : MonoBehaviour 
{

    public Timer lapTimer;
	public GUIText currentLapText;
	public GUIText fastestLapText;
    public float fastestTime = 0;
	public Recording fastestRecording;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
    {
        lapTimer.LapTimer();
		fastestLapText.text = "";
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		currentLapText.text = "Current Lap Time: " + lapTimer.currentTime.ToString("f2");
	}
	
	/// <summary>
	/// Fires when the ship crosses the finish line.
	/// </summary>
	/// <param name='collider'>
	/// The ship.
	/// </param>
    void OnTriggerEnter( Collider collider )
    {
		//checking to see when the ship crosses the finish line
        if( collider.transform.parent.tag == "Player" && transform.InverseTransformDirection( collider.attachedRigidbody.velocity ).z < 0 )
		{
			//sets your first lap as the fastest
			//or if your current lap is faster then your fastest, make your current lap the new fastest
			if( fastestTime == 0 || fastestTime > lapTimer.currentTime )
			{
				fastestTime = lapTimer.currentTime;
				Debug.Log( "New fastest time: " + fastestTime );
				
				//save the new ghost
				if( collider.transform.parent.GetComponent<Racer>().useVCR )
					fastestRecording = collider.transform.parent.GetComponent<InputVCR>().GetRecording();
			}
			
			//update fastest lap text
			fastestLapText.text = "Fastest Lap: " + fastestTime.ToString("f2");
			lapTimer.Reset();
            lapTimer.LapTimer();
			
			//set up ghost replay
			if( collider.transform.parent.GetComponent<Racer>().useVCR )
			{
				//reset recording
				collider.transform.parent.GetComponent<InputVCR>().NewRecording();
				
				//start fastest time recording
				GameObject.Find( "Ship1Ghost" ).GetComponent<GhostRacer>().StartReplay();
			}
		}
	}
}

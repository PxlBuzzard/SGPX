using UnityEngine;
using System.Collections;

/// <summary>
/// A ghost replay of a racer's best time.
/// </summary>
/// <author>Daniel Jost</author>
public class GhostRacer : MonoBehaviour 
{
	public InputVCR vcr;
	private Recording replay;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void StartReplay () 
	{
		//enable scripts
		GetComponent<Racer>().enabled = true;
		GetComponent<InputVCR>().enabled = true;
		
		//grab the fastest replay and play
		replay = GameObject.Find( "FinishLine" ).GetComponent<LapController>().fastestRecording;
		vcr.Play( replay, 0 );
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		//start ghost over again
		//if( replay != null && vcr.currentFrame > replay.totalFrames )
		//	vcr.Play( replay, 0 );
	}
}

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
	//private bool isPlaying = false;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void StartReplay () 
	{
		//enable scripts
		GetComponent<Racer>().enabled = true;
		GetComponent<InputVCR>().enabled = true;
		transform.GetComponentInChildren<MeshRenderer>().enabled = true;
		
		//grab the fastest replay and play
		replay = GameObject.Find( "FinishLine" ).GetComponent<LapController>().fastestRecording;
		vcr.Play( replay, 0 );
		vcr.finishedPlayback += replayFinished;
		//isPlaying = true;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
	}
	
	public void replayFinished ()
	{
		//stop replay
		vcr.Stop();
		
		//move to spawn position and hide the ship (for next replay)
		rigidbody.position = transform.position = GetComponent<Racer>().spawnPosition;
		rigidbody.velocity = Vector3.zero;
		transform.GetComponentInChildren<MeshRenderer>().enabled = false;
		//isPlaying = false;
		
		//disable script
		GetComponent<Racer>().enabled = false;
	}
}

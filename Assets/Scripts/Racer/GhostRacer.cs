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
	void Start()
	{
		//grab the live and ghost racer scripts
		Racer racerScript = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Racer>();
		Racer ghostScript = GetComponent<Racer>();
		
		//assign the live racer variables to the ghost
		ghostScript.acceleration = racerScript.acceleration;
		ghostScript.boostAccelerationMultiplier = racerScript.boostAccelerationMultiplier;
		ghostScript.boostMaxSpeed = racerScript.boostMaxSpeed;
		ghostScript.maxAngularVelocity = racerScript.maxAngularVelocity;
		ghostScript.maxRotation = racerScript.maxRotation;
		ghostScript.maxSpeed = racerScript.maxSpeed;
		ghostScript.rotateSpeed = racerScript.rotateSpeed;
		ghostScript.turnAcceleration = racerScript.turnAcceleration;
		rigidbody.constraints = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Rigidbody>().constraints;
	}
	
	/// <summary>
	/// Starts the replay.
	/// </summary>
	public void StartReplay () 
	{
		//enable scripts
		GetComponent<Racer>().enabled = true;
		GetComponent<InputVCR>().enabled = true;
		transform.GetComponentInChildren<MeshRenderer>().enabled = true;
		transform.GetComponentInChildren<ParticleSystem>().Play();
		
		//grab the fastest replay and play
		replay = GameObject.Find( "FinishLine" ).GetComponent<LapController>().fastestRecording;
		vcr.Play( replay, 0 );
		vcr.finishedPlayback += replayFinished;
	}
	
	/// <summary>
	/// Event that fires when the replay is finished.
	/// </summary>
	public void replayFinished ()
	{
		//stop replay
		vcr.Stop();
		
		//move to spawn position and hide the ship (for next replay)
		rigidbody.position = transform.position = GetComponent<Racer>().spawnPosition;
		rigidbody.velocity = Vector3.zero;
		transform.GetComponentInChildren<MeshRenderer>().enabled = false;
		transform.GetComponentInChildren<ParticleSystem>().Stop();
		
		//disable script
		GetComponent<Racer>().enabled = false;
	}
}

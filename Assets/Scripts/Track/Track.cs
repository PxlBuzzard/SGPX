using UnityEngine;
using System.Collections;

/// <summary>
/// A simple script to hold variables for a track.
/// </summary>
/// <author>Daniel Jost</author>
public class Track : MonoBehaviour 
{
	public string trackName;
	public int trackID;
	public Vector3 spawnPosition;
	public Vector3 spawnRotation;
	private GameObject racer;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		//grab the live ship
		racer = GameObject.FindGameObjectWithTag( "Player" );
		
		//set the spawn position
		racer.GetComponent<Racer>().SetSpawn( spawnPosition, spawnRotation );
		
		//reset the racer
		racer.GetComponent<Racer>().ResetLap();
	}
	
	/// <summary>
	/// Run custom code depending on the current track.
	/// </summary>
	public void TrackRules()
	{
		switch (transform.name)
		{
			case "DevTrack":
				DevTrack();
				break;
			case "SuperSpiral":
				SuperSpiral();
				break;
			case "TheLoop":
				TheLoop();
				break;
		}	
	}
	
	void DevTrack()
	{
		//disable x and z rotations
		racer.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	
	void SuperSpiral()
	{
		//disable x and z rotations
		racer.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		//enable gravity
		racer.rigidbody.useGravity = true;
		
	}
	
	void TheLoop()
	{
		//disable x and z rotations
		racer.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		//enable gravity
		//racer.rigidbody.useGravity = true;
		
	}
}
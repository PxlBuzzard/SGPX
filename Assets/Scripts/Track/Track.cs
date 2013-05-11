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
	public GameObject racer;
	public Vector3 spawnPosition;
	public Vector3 spawnRotation;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		//set the spawn position
		racer.GetComponent<Racer>().SetSpawn( spawnPosition, spawnRotation );
		
		//reset the racer
		racer.GetComponent<Racer>().ResetLap();
	}
}
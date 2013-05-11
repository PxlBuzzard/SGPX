using UnityEngine;
using System.Collections;

/// <summary>
/// A script to manage a checkpoint.
/// </summary>
/// <author>Pete O'Neal</author>
public class Checkpoint : MonoBehaviour 
{
	public LapController lapController;
	
	/// <summary>
	/// Fires when the ship crosses the checkpoint.
	/// </summary>
	/// <param name='collider'>
	/// The ship.
	/// </param>
    void OnTriggerEnter( Collider collider )
    {
		//checking to see when the ship crosses the finish line
        if( collider.transform.parent.tag == "Player" )
		{
			lapController.checkpointCounter++;
            lapController.Checkpoint();
		} 
	}
}

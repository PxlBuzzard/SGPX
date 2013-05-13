using UnityEngine;
using System.Collections;

/// <summary>
/// A script to manage a checkpoint.
/// </summary>
/// <author>Pete O'Neal</author>
/// <author>Daniel Jost</author>
public class Checkpoint : MonoBehaviour 
{
	public LapController lapController;
	public bool enableAllAxisRotations;
	public bool enableGravity;
	
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
		
		//set all rotations
		if ( enableAllAxisRotations )
		{
			collider.transform.parent.rigidbody.constraints = RigidbodyConstraints.None;	
		}
		else
		{
			collider.transform.parent.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;	
		}
		
		//set gravity
		collider.transform.parent.rigidbody.useGravity = enableGravity;	
	}
}

using UnityEngine;
using System.Collections;

public class CheckPoints : MonoBehaviour 
{
	public LapController lapController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


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
        if( collider.transform.parent.tag == "Player" )
		{
            lapController.Checkpoint();
		}
		
	}
}

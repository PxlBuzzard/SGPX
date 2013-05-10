using UnityEngine;
using System.Collections;

public class RacerUI : MonoBehaviour 
{
	public GameObject racer;
	[HideInInspector]
	public TextMesh lapTime;
	private GameObject velocityBar;
	private float forwardVelocity;
	private GameObject model;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		model = racer.GetComponent<Racer>().model;
	}
	
	public void Initialize ( bool isStatic )
	{
		if ( isStatic )
		{
			lapTime = transform.FindChild( "Main Camera" ).FindChild( "TimeText" ).gameObject.GetComponent<TextMesh>();
			velocityBar = transform.FindChild( "Main Camera" ).FindChild( "Velocity Bar" ).gameObject;
			lapTime.GetComponent<MeshRenderer>().enabled = velocityBar.GetComponent<MeshRenderer>().enabled = true;	
		}
		else
		{
			lapTime = model.transform.FindChild( "TimeText" ).gameObject.GetComponent<TextMesh>();
			velocityBar = model.transform.FindChild( "Velocity Bar" ).gameObject;
			lapTime.GetComponent<MeshRenderer>().enabled = velocityBar.GetComponent<MeshRenderer>().enabled = true;	
		}
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		//get forward velocity
		forwardVelocity = racer.transform.InverseTransformDirection( racer.rigidbody.velocity ).z;
		
		//update velocity graphic
		velocityBar.renderer.material.SetFloat( "_Progress", (float)( forwardVelocity / racer.GetComponent<Racer>().maxSpeed ) );
		
		// Display velocity (the old way)
		//if( velocity )
        //	velocity.text = Mathf.RoundToInt( forwardVelocity ).ToString();
	}
}

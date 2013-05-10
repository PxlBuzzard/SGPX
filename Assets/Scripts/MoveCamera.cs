using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	
	public GameObject creditPosition;
	public GameObject mainPosition;
	public GameObject inputPosition;
	
	public float moveSpeed;
	
	public enum CamPositions
	{
		credits,
		main,
		input
		
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ChangeCamera(CamPositions newPos)
	{
		if(newPos == CamPositions.credits)
		{
			//this.transform.position=Vector3.Lerp(mainPosition.transform.position,creditPosition.transform.position,Time.deltaTime*moveSpeed);
			this.transform.position=creditPosition.transform.position;
			this.transform.rotation=creditPosition.transform.rotation;
		}
		 else if (newPos == CamPositions.main)
		{
			this.transform.position=mainPosition.transform.position;
			this.transform.rotation=mainPosition.transform.rotation;
		}
		else if(newPos == CamPositions.input)
		{
			this.transform.position=inputPosition.transform.position;
			this.transform.rotation=inputPosition.transform.rotation;
		}
	}
}

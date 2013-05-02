using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	
	public GameObject camPos;
	public GameObject mainPosition;
	
	public enum CamPositions
	{
		credits,
		main
		
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
			this.transform.position=camPos.transform.position;
			this.transform.rotation=camPos.transform.rotation;
		}
		 else if (newPos == CamPositions.main)
		{
			this.transform.position=mainPosition.transform.position;
			this.transform.rotation=mainPosition.transform.rotation;
		}
	}
}

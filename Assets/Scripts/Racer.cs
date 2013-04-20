using UnityEngine;
using System.Collections;

public class Racer : MonoBehaviour 
{
	float speed = 1.0f;
	float rotateSpeed = 3.0f;
	Rigidbody vehicle;
	
	// Use this for initialization
	void Start () 
	{
		vehicle = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0, 0, Input.GetAxis("Horizontal") * rotateSpeed);
		
		Vector3 moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		moveDirection *= speed;
	    //transform.TransformDirection(moveDirection);
		
		vehicle.AddForce(moveDirection);
	}
}

using UnityEngine;
using System.Collections;

public class RacerCamera : MonoBehaviour
{
    public Racer target;
    public float rotationSpeed;
    public float maxRotation;
    public float downAngleMultiplier;
    private Vector3 initialRotation;
    private Vector3 initialPosition;
    private float prevVelocity;

    // Use this for initialization
    void Start()
    {
        initialRotation = transform.localEulerAngles;
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // Rotate camera
        float rotationAngle = 0.0f;
        if( Input.GetAxis( "Horizontal" ) != 0.0f )
        {
            if( ( Input.GetAxis( "Horizontal" ) > 0.0f && ( transform.localEulerAngles.y < maxRotation || transform.localEulerAngles.y > 360 - maxRotation - 10.0f ) ) ||
                ( Input.GetAxis( "Horizontal" ) < 0.0f && ( transform.localEulerAngles.y > 360 - maxRotation || transform.localEulerAngles.y < maxRotation + 10.0f ) ) )
            {
                rotationAngle = Input.GetAxis( "Horizontal" ) * rotationSpeed * Time.deltaTime;
            }
        }
        else
        {
            if( transform.localEulerAngles.y < 0.5f || transform.localEulerAngles.y > 359.5f )
                rotationAngle = 0.0f;
            else if( transform.localEulerAngles.y > 360 - maxRotation - 10.0f )
                rotationAngle = rotationSpeed * Time.deltaTime;
            else if( transform.localEulerAngles.y < maxRotation + 10.0f )
                rotationAngle = -rotationSpeed * Time.deltaTime;
        }
        transform.RotateAround( target.transform.position, target.transform.up, rotationAngle );

        // Reset camera if ship is going straight
        if( Mathf.Abs( transform.localPosition.x ) < 0.1f )
        {
            transform.localEulerAngles = new Vector3( transform.localEulerAngles.x, initialRotation.y, transform.localEulerAngles.z );
            transform.localPosition = new Vector3( initialPosition.x, transform.localPosition.y, transform.localPosition.z );
        }

        // Rotate camera down with speed
        transform.RotateAround( target.transform.position, target.transform.right, ( prevVelocity - transform.InverseTransformDirection( target.rigidbody.velocity ).z ) / downAngleMultiplier );

        // Keep track of car velocity
        prevVelocity = transform.InverseTransformDirection( target.rigidbody.velocity ).z;

        // Keep camera from twisting out of control
        transform.localEulerAngles = new Vector3( transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f );
    }
}
using UnityEngine;
using System.Collections;

public class RacerCamera : MonoBehaviour
{
    public Racer target;
    public float rotationSpeed;
    public float maxRotation;
    public float damping;
    public float angleMultiplier;
    //private Quaternion previousRotation;
    private Vector3 initialRotation;
    private Vector3 initialPosition;
    private float prevVelocity;

    // Use this for initialization
    void Start()
    {
        //previousRotation = transform.rotation;
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
        transform.RotateAround( target.transform.position, Vector3.up, rotationAngle );

        // Reset camera if ship is going strait
        if( Mathf.Abs( transform.localPosition.x ) < 0.1f )
        {
            transform.localEulerAngles = new Vector3( transform.localEulerAngles.x, initialRotation.y, transform.localEulerAngles.z );
            transform.localPosition = new Vector3( initialPosition.x, transform.localPosition.y, transform.localPosition.z );
        }

        // Rotate camera down with speed
        transform.RotateAround( target.transform.position, target.transform.right, ( prevVelocity - target.rigidbody.velocity.magnitude ) / angleMultiplier );

        // Keep track of car velocity
        prevVelocity = target.rigidbody.velocity.magnitude;

        // Don't ask.
        transform.localEulerAngles = new Vector3( transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f );
    }
}
using UnityEngine;

public class Racer : MonoBehaviour
{
    public float acceleration;
    public float rotateSpeed;
    public float maxSpeed;
    public float boostAccelerationMultiplier;
    public float boostMaxSpeed;
    public float maxAngularVelocity;
    public float maxRotation;
    public float turnAcceleration;
    public GUIText velocity;
    public GameObject model;
    public Rigidbody rigidBody;
    public ParticleSystem particleSystem;
    private float currentTurn;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isBoosting = Input.GetAxis( "Boost" ) > 0.0f;

        // Lean
        if( Input.GetAxis( "Horizontal" ) < 0.0f )
            currentTurn = Mathf.Max( new float[] { Input.GetAxis( "Horizontal" ), currentTurn - ( turnAcceleration * Time.deltaTime ) } );
        else if( Input.GetAxis( "Horizontal" ) > 0.0f )
            currentTurn = Mathf.Min( new float[] { Input.GetAxis( "Horizontal" ), currentTurn + ( turnAcceleration * Time.deltaTime ) } );
        else if( Mathf.Abs( currentTurn ) > 0.2f )
            currentTurn = currentTurn + ( ( currentTurn > 0.0f ? -turnAcceleration : turnAcceleration ) * Time.deltaTime );
        else
            currentTurn = 0.0f;
        model.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, maxRotation * currentTurn );

        // Rotate
        rigidBody.AddTorque( rigidBody.transform.up * -currentTurn * rotateSpeed );
        rigidBody.velocity = rigidBody.transform.forward * rigidBody.velocity.magnitude;
        if( rigidBody.angularVelocity.magnitude > maxAngularVelocity )
            rigidBody.angularVelocity = rigidBody.angularVelocity.normalized * maxAngularVelocity;

        // Accelerate
        rigidBody.AddRelativeForce( 0.0f, 0.0f, ( Input.GetAxis( "Vertical" ) * acceleration * Time.deltaTime * ( isBoosting ? boostAccelerationMultiplier : 1.0f ) ) );
        if( !isBoosting && rigidBody.velocity.magnitude > maxSpeed + 10.0f )
            rigidbody.velocity = rigidbody.velocity.normalized * ( rigidbody.velocity.magnitude - 10.0f );
        else if( !isBoosting && rigidBody.velocity.magnitude > maxSpeed )
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        else if( isBoosting && rigidbody.velocity.magnitude > boostMaxSpeed )
            rigidBody.velocity = rigidBody.velocity.normalized * boostMaxSpeed;

        // Set particles!
        //particleSystem.startSpeed = ( rigidBody.velocity.magnitude / maxSpeed ) * 10.0f;
        particleSystem.emissionRate = ( rigidBody.velocity.magnitude / maxSpeed ) * 400.0f;

        // Display velocity
        velocity.text = "Velocity: " + Mathf.RoundToInt( rigidBody.velocity.magnitude ).ToString() + "\n" +
            "Angular Velocity: " + rigidBody.angularVelocity.magnitude.ToString() + "\n" +
            "Current Turn: " + currentTurn;
    }
}
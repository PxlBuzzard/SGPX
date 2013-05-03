using UnityEngine;

/// <summary>
/// Handle functions related to the race vehicle.
/// </summary>
/// <author>Colden Cullen</author>
/// <author>Daniel Jost</author>
public class Racer : MonoBehaviour
{
	#region Class Variables
	public bool useStaticUI;
    public float acceleration;
    public float rotateSpeed;
    public float maxSpeed;
    public float boostAccelerationMultiplier;
    public float boostMaxSpeed;
    public float maxAngularVelocity;
    public float maxRotation;
    public float turnAcceleration;
    public GameObject model;
    public ParticleSystem engineParticles;
    public GameObject[] raycasters;
	public bool useVCR;
	public InputVCR vcr;
    public TextMesh splitTimeText;
	public Vector3 spawnPosition;
	public ParticleSystem speedLines;
	public RacerUI ui;
    private float currentTurn;
    private RaycastHit[] raycastHits;
	private Quaternion spawnRotation;
	private bool isBoosting = false;
	private bool isPaused = false;
	#endregion

    /// <summary>
    /// Constructor.
    /// </summary>
    void Start()
    {
		//set gravity on ship
        Physics.gravity = new Vector3( 0, -250, 0 );
		
		//disable collision with ghost replay
		if ( name == "Ship1" )
       		Physics.IgnoreCollision( model.collider, GameObject.Find( "Ship1Ghost" ).GetComponent<Racer>().model.collider );
		
		//initialize raycasts
        raycastHits = new RaycastHit[ raycasters.Length ];
		
		//set up UI hooks
		if ( ui )
			ui.Initialize( useStaticUI );

		//set spawn location on the track (TEMPORARY, ONLY WORKS FOR ONE TRACK)
	    spawnPosition = new Vector3( 0.0f, -119.242f, 363.0434f );
		spawnRotation = new Quaternion( 0.0f, 180f, 0.0f, 1.0f );
		
		//SHIV TRACK SPAWN
		//spawnPosition = new Vector3(-179.4068f, 34.04115f, -44.46555f);
		//spawnRotation = new Quaternion(0.0f, 243f, 0.0f, 1.0f);
		
		//make the ship update its physics like hella
        rigidbody.solverIterationCount = 20;
    }

    /// <summary>
    /// Updates on a fixed time interval.
    /// </summary>
    void FixedUpdate()
    {
		//Check for lap reset button
		if( vcr.GetButtonDown( "Reset" ) )
			ResetLap();
		else if( Input.GetButtonDown( "Reset" ) )
			ResetLap();
		
		//Check for pause button, doesn't actually work yet
		if( vcr.GetButtonDown( "Reset" ) )
		{
			Time.timeScale = 0;
			isPaused = true;
		}
		else if( Input.GetButtonDown( "Reset" ) && isPaused )
		{
			Time.timeScale = 1;
			isPaused = false;
		}
		
		//Boost
		//if( useVCR )
			//isBoosting = vcr.GetAxis( "Boost" ) > 0.0f;
		//else
        	//isBoosting = Input.GetAxis( "Boost" ) > 0.0f;

        // Rotate to match plane
        // 0: Front
        // 1: Back
        // 2: Left
        // 3: Right
		
        // Do raycasts
        for( int ii = 0; ii < raycasters.Length; ++ii )
            Physics.Raycast( raycasters[ ii ].transform.position, -raycasters[ ii ].transform.up, out raycastHits[ ii ] );

        //Debug.Log( raycastHits[ 0 ].distance + ", " + raycastHits[ 1 ].distance + "  /  " + raycastHits[ 2 ].distance + ", " + raycastHits[ 3 ].distance );
		
		//hover off the ground
        if( raycastHits[ 0 ].distance < 1.5f )
            rigidbody.AddForce( 0.0f, 100.0f - raycastHits[ 0 ].distance, 0.0f );
		if( raycastHits[ 1 ].distance < 1.5f )
            rigidbody.AddForce( 0.0f, 100.0f - raycastHits[ 1 ].distance, 0.0f );

        // If on the track
        if( raycastHits[ 0 ].distance < 1.5f || raycastHits[ 1 ].distance < 1.5f )
        {
            // If the front and back of the ship are not balanced, rotate to balance them
            if( raycastHits[ 0 ].distance != raycastHits[ 1 ].distance )
                transform.Rotate(
                    Mathf.Atan( ( raycastHits[ 0 ].distance - raycastHits[ 1 ].distance ) / ( raycasters[ 1 ].transform.localPosition.z - raycasters[ 0 ].transform.localPosition.z ) ),
                    0.0f, 0.0f );
            
            // Rotate up onto berms
            if( Mathf.Abs( raycastHits[ 2 ].distance - raycastHits[ 3 ].distance ) > 0.05f )
                transform.Rotate(
                    0.0f, 0.0f,
                    Mathf.Atan( ( raycastHits[ 2 ].distance - raycastHits[ 3 ].distance ) / ( raycasters[ 2 ].transform.localPosition.z - raycasters[ 3 ].transform.localPosition.z ) ) );
        }
		//if going over a jump or off the track, turn the noise downwards
        else
        {
            transform.Rotate( rigidbody.mass / 5.0f, 0.0f, 0.0f );
        }
        
        // Lean
		if( useVCR )
		{
	        if( vcr.GetAxis( "Horizontal" ) < 0.0f )
	            currentTurn = Mathf.Max( new float[] { vcr.GetAxis( "Horizontal" ), currentTurn - ( turnAcceleration * Time.deltaTime ) } );
	        else if( vcr.GetAxis( "Horizontal" ) > 0.0f )
	            currentTurn = Mathf.Min( new float[] { vcr.GetAxis( "Horizontal" ), currentTurn + ( turnAcceleration * Time.deltaTime ) } );
	        else if( Mathf.Abs( currentTurn ) > 0.2f )
	            currentTurn = currentTurn + ( ( currentTurn > 0.0f ? -turnAcceleration : turnAcceleration ) * Time.deltaTime );
	        else
	            currentTurn = 0.0f;
		}
		else
		{
			if( Input.GetAxis( "Horizontal" ) < 0.0f )
	            currentTurn = Mathf.Max( new float[] { Input.GetAxis( "Horizontal" ), currentTurn - ( turnAcceleration * Time.deltaTime ) } );
	        else if( Input.GetAxis( "Horizontal" ) > 0.0f )
	            currentTurn = Mathf.Min( new float[] { Input.GetAxis( "Horizontal" ), currentTurn + ( turnAcceleration * Time.deltaTime ) } );
	        else if( Mathf.Abs( currentTurn ) > 0.2f )
	            currentTurn = currentTurn + ( ( currentTurn > 0.0f ? -turnAcceleration : turnAcceleration ) * Time.deltaTime );
	        else
	            currentTurn = 0.0f;
		}
        model.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, maxRotation * currentTurn );

        float forwardVelocity = transform.InverseTransformDirection( rigidbody.velocity ).z;

        // Rotate
        rigidbody.AddTorque( rigidbody.transform.up * -currentTurn * rotateSpeed );
        rigidbody.velocity = rigidbody.transform.forward * forwardVelocity;
        if( rigidbody.angularVelocity.magnitude > maxAngularVelocity )
            rigidbody.angularVelocity = rigidbody.angularVelocity.normalized * maxAngularVelocity;

        // Accelerate
		if( useVCR )
			rigidbody.AddForce( rigidbody.transform.forward * ( vcr.GetAxis( "Vertical" ) * acceleration * Time.deltaTime * ( isBoosting ? boostAccelerationMultiplier : 1.0f ) ) );
		else
        	rigidbody.AddForce( rigidbody.transform.forward * ( Input.GetAxis( "Vertical" ) * acceleration * Time.deltaTime * ( isBoosting ? boostAccelerationMultiplier : 1.0f ) ) );
        if( !isBoosting && forwardVelocity > maxSpeed + 10.0f )
            rigidbody.velocity = rigidbody.velocity.normalized * ( forwardVelocity - 10.0f );
        else if( !isBoosting && forwardVelocity > maxSpeed )
		{
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
			speedLines.Play();
		}
        else if( isBoosting && forwardVelocity > boostMaxSpeed )
		{
            rigidbody.velocity = rigidbody.velocity.normalized * boostMaxSpeed;
			speedLines.Play();
		}
		else
		{
			speedLines.Stop();
		}

        // Change particles based on input
        if( useVCR )
            engineParticles.emissionRate = ( vcr.GetAxis( "Vertical" ) + ( isBoosting ? 1.0f : 0.0f ) ) * 400.0f;
        else
            engineParticles.emissionRate = ( Input.GetAxis( "Vertical" ) + ( isBoosting ? 1.0f : 0.0f ) ) * 400.0f;
    }
	
	/// <summary>
	/// Fires on the first collision with a trigger collider.
	/// </summary>
	/// <param name='collider'>
	/// Object being collided with.
	/// </param>
	void OnTriggerEnter(Collider collider)
	{
		//if the ship hits the track floor, reset the player
		if( collider.name == "TrackFloor" )
		{
			ResetLap();
		}
	}
	
	/// <summary>
	/// Resets the lap.
	/// </summary>
	void ResetLap()
	{
		//reset position/rotation
		transform.position = spawnPosition;
		transform.rotation = spawnRotation;
		
		//reset velocities
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.velocity = Vector3.zero;
		
		//reset the lap timer
		GameObject.Find( "FinishLine" ).GetComponent<Timer>().Reset();
		
		//kill the ghost
		GameObject.Find( "Ship1Ghost" ).GetComponent<GhostRacer>().replayFinished();
	}
}
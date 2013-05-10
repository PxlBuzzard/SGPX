using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    public GameObject creditPosition;
    public GameObject mainPosition;
    public GameObject inputPosition;
    public float transitionTime;

    private GameObject targetPosition;
    private CamPositions currentState;
    private CamPositions newState;
    private float startTime;

    public enum CamPositions
    {
        Credits,
        Main,
        Input,
        Transition
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( currentState == CamPositions.Transition )
        {
            transform.position = Vector3.Slerp( transform.position, targetPosition.transform.position, ( Time.time - startTime ) / transitionTime );
            transform.rotation = Quaternion.Slerp( transform.rotation, targetPosition.transform.rotation, ( Time.time - startTime ) / transitionTime );

            if( ( Time.time - startTime ) / transitionTime >= 1.0f )
                currentState = newState;
        }
    }

    public void ChangeCamera( CamPositions newPos )
    {
        currentState = CamPositions.Transition;
        startTime = Time.time;

        switch( newPos )
        {
            case CamPositions.Credits:
                targetPosition = creditPosition;
                newState = CamPositions.Credits;
                break;
            case CamPositions.Main:
                targetPosition = mainPosition;
                newState = CamPositions.Main;
                break;
            case CamPositions.Input:
                targetPosition = inputPosition;
                newState = CamPositions.Input;
                break;
        }
    }
}
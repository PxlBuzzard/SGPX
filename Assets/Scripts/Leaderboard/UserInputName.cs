using UnityEngine;
using System.Collections;

public class UserInputName : MonoBehaviour
{
	//constants
	private const string SECRET_KEY = "mySecretKey";
	private const string GET_USER_URL = "http://sgpx.coldencullen.com/php/getuser.php?";
	string hash = SECRET_KEY;
	
	// variables
	private string playerName = "";
	public GUIStyle TextStyle = new GUIStyle();
	public int fontSize; 

	public string PlayerName
	{
		get
		{
			return this.playerName;
		}
	}

	//bool showGUI = false;
	// Use this for initialization
	void Start ()
	{
	
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	// draw the gui for the player to input their name
	void OnGUI()
	{
		// adjusts as the screen size changes
		// helps the textfield stay in the same place (somewhat)
		float halfScH = Screen.height/2;
		float halfScW = Screen.width/2;
		//textfield for player in input their name
		playerName = GUI.TextField( new Rect((halfScW/2),( halfScH),(halfScW),(halfScH/4)), playerName, TextStyle );
	}
}

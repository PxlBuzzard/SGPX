using UnityEngine;
using System.Collections;

public class UserInputName : MonoBehaviour
{
	private const string SECRET_KEY = "mySecretKey";
	private const string GET_USER_URL = "http://sgpx.coldencullen.com/php/getuser.php?";
	string hash = SECRET_KEY;
	WWW getName;
	
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
	
	void OnGUI()
	{
		playerName = GUI.TextField( new Rect(300,250,670,100), playerName, TextStyle );
	}
}

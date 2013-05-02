using UnityEngine;
using System.Collections;

public class UserInputName : MonoBehaviour {
	
	string playerName = "";
	public GUIStyle TextStyle = new GUIStyle();
	public int fontSize;
	//bool showGUI = false;
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		playerName = GUI.TextField (new Rect (300,250,670,100),playerName,TextStyle);
		
		
		
	}
}

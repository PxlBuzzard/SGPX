using UnityEngine;
using System.Collections;

public class ButtonHover : MonoBehaviour
{
	// Leaderboard values
	private const string SECRET_KEY = "mySecretKey";
	private const string GET_USER_URL = "http://sgpx.coldencullen.com/php/getuser.php?";
	string hash = SECRET_KEY;
	WWW getName;
	
	//variables
	public GameObject[] buttons;
	public GameObject[] selectorIcon;
	public GameObject[] backArray;
	public GameObject[] goArray;
	public UserInputName textField;
	
	int index=0;
	int prevIndex=0;
	
	bool mainMenu = true;
	bool btnReset = true;
	bool backBtn = false;
	bool playGame = false;

	
	// Use this for initialization
	void Start () {
		// have the play button be highlighted when openning the game
		buttons[0].renderer.material.color= Color.cyan;
		//  hard codes which little circles show up next to the buttons in when openning
		selectorIcon[0].renderer.enabled =true;
		selectorIcon[1].renderer.enabled =false;
		selectorIcon[2].renderer.enabled =false;
		
		
		
		// highlights the back and go buttons
		backArray[0].renderer.material.color = Color.cyan;
		goArray[0].renderer.material.color = Color.cyan;
		//backArray[1].renderer.enabled = true;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		hoverOver();
		selectBtn ();
		
								GUI.SetNextControlName("playerName");
				GUI.FocusControl("playerName");
	}
	
	// Shows which button is selected
	void hoverOver()
	{
		// if on the main menu
		if (mainMenu)
		{
			if(btnReset)
			{
				prevIndex=index;
				// handles the switching between buttons
				if(Input.GetAxis("Vertical")>0)
				{
					
					index--;
					if(index < 0)
					{
						index= buttons.Length -1;
					}
					
					selectorIcon[index].renderer.enabled = true;
					selectorIcon[prevIndex].renderer.enabled =false;
					
					buttons[index].renderer.material.color= Color.cyan;
					buttons[prevIndex].renderer.material.color= Color.white;
					
					btnReset=false;
				}
				else if(Input.GetAxis ("Vertical")<0)
				{	
					index++;
					if(index >= buttons.Length)
					{
						index = 0;
					}
				
					selectorIcon[prevIndex].renderer.enabled =false;
					selectorIcon[index].renderer.enabled = true;
					
					buttons[prevIndex].renderer.material.color= Color.white;
					buttons[index].renderer.material.color= Color.cyan;
					btnReset=false;
				}
			}
			// resets the buttons so you can infinitly scroll through them
			else if(Input.GetAxis("Vertical")==0)
			{
				btnReset=true;
			}
		}
	}
	
	// helps with button selection
	void selectBtn()
	{
		if(Input.GetButtonUp("select"))
		{
			// back button moves camera back to main menu
			if (backBtn)
			{
				GameObject.Find("Main Camera").GetComponent<MoveCamera>().ChangeCamera(MoveCamera.CamPositions.Main);
				mainMenu = true;
				backBtn = false;
				playGame = false;
				
			}
			// plays the game
			else if (playGame)
			{
				getName = new WWW(
		            GET_USER_URL +
		            "name=" + WWW.EscapeURL( textField.PlayerName ) +
		            "&hash=" + hash
		        );
				
				while( !getName.isDone );
				
				UserIDController.Instance.userID = int.Parse( getName.text );
				
				MonoBehaviour btn = goArray[ 0 ].GetComponent( "hover Script" ) as MonoBehaviour;
				btn.SendMessage( "ButtonSelected", SendMessageOptions.RequireReceiver );
			}
			// goes to the credits 
			else if(index == 1)
			{
				GameObject.Find("Main Camera").GetComponent<MoveCamera>().ChangeCamera(MoveCamera.CamPositions.Credits);
				mainMenu = false; 
				backBtn = true;
				playGame = false;
	
			}
			// goes to player input page
			else if(index == 0)
			{
				GameObject.Find("Main Camera").GetComponent<MoveCamera>().ChangeCamera(MoveCamera.CamPositions.Input);
				//GameObject.Find ("GUI Text").GetComponent<UserInputName>().OnGUI(showGUI);
				GUI.FocusControl("playerName");
				textField.enabled=true;
				
				mainMenu = false;
				backBtn = false;
				playGame = true;
				
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class ButtonHover : MonoBehaviour {
	//variables
	public GameObject[] buttons;
	public GameObject[] selectorIcon;
	public GameObject[] backArray;
	int index=0;
	int backIndex=0;
	bool mainMenu = true;
	int prevIndex=0;
	bool btnReset = true;
	bool backBtn = false;
	
	// Use this for initialization
	void Start () {
		// have the play button be highlighted when openning the game
		buttons[0].renderer.material.color= Color.cyan;
		selectorIcon[0].renderer.enabled =true;
		selectorIcon[1].renderer.enabled =false;
		selectorIcon[2].renderer.enabled =false;
		
		backArray[0].renderer.material.color = Color.cyan;
		//backArray[1].renderer.enabled = true;
		
		/*index = 2;
		prevIndex = 2;*/
		
	}
	
	// Update is called once per frame
	void Update () {
		
		hoverOver();
		selectBtn ();
	}
	
	// Shows which button is selected
	void hoverOver()
	{
		if (mainMenu)
		{
			if(btnReset)
			{
				prevIndex=index;
				//RaycastHit press;
				//bool hit = Physics.Raycast(Camera.mainCamera.ScreenPointToRay(Input.mousePosition), out press);
				
				// handles the switching between buttons
				if(Input.GetAxis("Vertical")>0)
				{
					/*
					index--;
					if(index >= buttons.Length)
					{
						index = 0;
					}
					*/
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
					//*
					index++;
					if(index >= buttons.Length)
					{
						index = 0;
					}
					/*/
					index++;
					if(index < 0)
					{
						index= buttons.Length -1;
					}
					//*/
					selectorIcon[prevIndex].renderer.enabled =false;
					selectorIcon[index].renderer.enabled = true;
					
					buttons[prevIndex].renderer.material.color= Color.white;
					buttons[index].renderer.material.color= Color.cyan;
					btnReset=false;
				}
			}
			else if(Input.GetAxis("Vertical")==0)
			{
				btnReset=true;
			}
		}
	}
	
	void selectBtn()
	{
		if(Input.GetButtonUp("select"))
		{
			if (backBtn)
			{
				GameObject.Find("Main Camera").GetComponent<MoveCamera>().ChangeCamera(MoveCamera.CamPositions.main);
				mainMenu = true;
				backBtn = false;
				
			}
			else if(index == 1)
			{
				GameObject.Find("Main Camera").GetComponent<MoveCamera>().ChangeCamera(MoveCamera.CamPositions.credits);
				mainMenu = false;
				backBtn = true;
				
			}
			else
			{
				MonoBehaviour btn = buttons[index].GetComponent("hover Script") as MonoBehaviour;
				btn.SendMessage("ButtonSelected",SendMessageOptions.RequireReceiver);
			}
			
			/* if(backArray[0])
			{
				GameObject.Find("Main Camera").GetComponent<MoveCamera>().ChangeCamera(MoveCamera.CamPositions.main);	
			}*/
		}
	}
}

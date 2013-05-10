using UnityEngine;
using System.Collections;

public class UserIDController
{
	private static UserIDController instance;
	
	public int userID;
	
	public static UserIDController Instance
	{
		get
		{
			if( instance == null )
				instance = new UserIDController();
			
			return instance;
		}
	}
	
	// Use this for initialization
	private UserIDController()
	{
		if( instance != null )
			return;
		
		instance = this;
	}
}
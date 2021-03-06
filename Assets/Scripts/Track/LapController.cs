#region Using Statements
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using LitJson;
#endregion

/// <summary>
/// A script to handle ghosts, leaderboards, and checkpoints.
/// </summary>
/// <author>Pete O'Neal</author>
/// <author>Daniel Jost</author>
/// <author>Colden Cullen</author>
public class LapController : MonoBehaviour 
{
	#region Constants
    private const string SECRET_KEY = "mySecretKey";
    private const string GET_SCORES_URL = "http://sgpx.coldencullen.com/php/getscores.php?";
    private const string ADD_SCORE_URL = "http://sgpx.coldencullen.com/php/addscore.php?";
	private const string FTP_USER = "sgpx";
	private const string FTP_PASSWORD = "superracr";
	#endregion
	
	#region Variables
    public bool uploadTimes;
    public Timer lapTimer;
    public Timer delayTimer;
	public GameObject racer;
    public GUIText fastestLapText;
    public GUIText leaderboardText;
    public GameObject[] checkpoints;
	[HideInInspector]
    public int checkpointCounter = 0;
	[HideInInspector]
    public float fastestTime = 0;
	public Recording fastestRecording;
    private WWW upload;
    private bool waitingForUpload = false;
	private int trackID;
	private TextMesh lapTime;
	#endregion

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() 
    {
		fastestLapText.text = "";
		
		lapTime = GetComponentInChildren<TextMesh>();
		
		//get the track ID from the parent Track script
		trackID = transform.parent.GetComponent<Track>().trackID;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		//update the current time of the lap
		if( racer.tag == "Player" )
			racer.GetComponent<RacerUI>().lapTime.text = lapTimer.currentTime.ToString( "f3" );
		
		//update the lap time on the track (if it's a thing)
		if ( lapTime )
			lapTime.text = lapTimer.currentTime.ToString( "f3" );
		
		//upload scores to the server
        if( fastestRecording != null && upload != null && waitingForUpload )
        {
            if( upload.isDone )
            {
                waitingForUpload = false;

                if( !String.IsNullOrEmpty( upload.text ) )
                {
                    StartCoroutine( UploadGhost() );
                }
            }
        }
		
		//hide the split time text after the timer is up
        if ( delayTimer.isFinished )
        {
            racer.GetComponent<Racer>().splitTimeText.text = "";
        }
	}
	
	/// <summary>
	/// Uploads the ghost replay JSON file to a server.
	/// </summary>
    IEnumerator UploadGhost()
    {
        // Get object used to communicate with server
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create( "ftp://buckeye.dreamhost.com/sgpx.coldencullen.com/ghosts/" + upload.text );
        request.Method = WebRequestMethods.Ftp.UploadFile;

        // Login
		//NOTE: Hardcoding our login credentials here is not a smart idea.
        request.Credentials = new NetworkCredential( FTP_USER, FTP_PASSWORD );

        // Upload to server
        byte[] fileContents = Encoding.UTF8.GetBytes( fastestRecording.ToString() );
        request.ContentLength = fileContents.Length;

        Stream requestStream = request.GetRequestStream();
        requestStream.Write( fileContents, 0, fileContents.Length );
        requestStream.Close();

        Debug.Log( ( request.GetResponse() as FtpWebResponse ).StatusDescription );

        yield return null;
    }
	
	/// <summary>
	/// Fires when the ship crosses the finish line.
	/// </summary>
	/// <param name='collider'>
	/// The ship.
	/// </param>
    void OnTriggerEnter( Collider collider )
    {
		//checking to see when the ship crosses the finish line
        if( collider.transform.parent.tag == "Player" && transform.InverseTransformDirection( collider.attachedRigidbody.velocity ).z < 0 )
		{
			//start the lap timer if it hasn't started
			if( lapTimer.currentTime == 0.0f )
			{
				lapTimer.LapTimer();
				
				//start recording input at beginning of the lap
				if( racer.GetComponent<Racer>().useVCR && fastestRecording != null )
				{
					//start fastest time recording
	                GameObject.Find( "Ship1Ghost" ).GetComponent<GhostRacer>().StartReplay();
				}
				else if( racer.GetComponent<Racer>().useVCR )
				{
					racer.GetComponent<Racer>().vcr.NewRecording();
				}
			}
			else
			{
                //check to see if they passed through all 3 checkpoints
                if (checkpointCounter == checkpoints.Length)
                {
                    checkpointCounter = 0;

                    float lapTime = lapTimer.currentTime;

                    //sets your first lap as the fastest
                    //or if your current lap is faster then your fastest, make your current lap the new fastest
                    if (fastestTime == 0 || fastestTime > lapTime)
                    {
                        fastestTime = lapTime;
                        Debug.Log("New fastest time: " + fastestTime);

                        //save the new ghost
                        if (collider.transform.parent.GetComponent<Racer>().useVCR)
                            fastestRecording = collider.transform.parent.GetComponent<InputVCR>().GetRecording();
                    }

                    //update fastest lap text
                    fastestLapText.text = "Fastest Lap: " + fastestTime.ToString("f2");
                    lapTimer.Reset();
                    lapTimer.LapTimer();

                    // Send time to database
                    //string hash = Md5Sum( playerName + lapTime + SECRET_KEY );
                    string hash = SECRET_KEY;

                    if (uploadTimes && UserIDController.Instance.userID > 0)
                    {
                        upload = new WWW(
                            ADD_SCORE_URL +
                            "userID=" + UserIDController.Instance.userID +
                            "&trackID=" + trackID +
                            "&time=" + (Mathf.Round(lapTime * 1000.0f) / 1000.0f) +
                            "&hash=" + hash
                        );
                        Debug.Log(upload.url);

                        waitingForUpload = true;
                    }


                    //set up ghost replay
                    if (collider.transform.parent.GetComponent<Racer>().useVCR)
                    {
                        //reset recording
                        collider.transform.parent.GetComponent<InputVCR>().NewRecording();

                        //start fastest time recording
                        GameObject.Find("Ship1Ghost").GetComponent<GhostRacer>().StartReplay();
                    }
                }
				//if the player did not pass through all the checkpoints then this code is executed
				else
				{
					delayTimer.Countdown(5.0f);
					racer.GetComponent<Racer>().splitTimeText.text = "You missed a checkpoint. That lap will not count!";
					checkpointCounter = 0;
					lapTimer.Reset();
                    lapTimer.LapTimer();
				}
			}
		}		
    }
	
	/// <summary>
	/// Fires when the racer crosses a checkpoint.
	/// </summary>
	public void Checkpoint ()
	{
		//have a list of checkpoints, and check to see if you have passed them in numerical order	
        if (racer.GetComponent<Racer>().splitTimeText)
        {
            delayTimer.Countdown(3.0f);
            racer.GetComponent<Racer>().splitTimeText.text = "CHECK POINT TIME: " + lapTimer.currentTime.ToString("f3");
        }
	}
	
    /// <summary>
    /// Updates the scoreboard online.
    /// </summary>
    private void UpdateScoreboard()
    {
        // Request data
        WWW bestTimes = new WWW( GET_SCORES_URL + "track=" + trackID );
        while (!bestTimes.isDone) ;

        // Parse data to array of JSON objects
        JsonData data = JsonMapper.ToObject( bestTimes.text );

        // Create array to pull results from
        LeaderboardEntry[] results = new LeaderboardEntry[ data.Count ];

        // Convert JSON data to C# objects
        for( int ii = 0; ii < data.Count; ++ii )
        {
            results[ ii ] = JsonMapper.ToObject<LeaderboardEntry>( data[ ii ].ToJson() );
            Debug.Log( ii + ", " + data[ ii ].ToJson() + ", " + results[ ii ].Name );
        }

        // Display results
        leaderboardText.text = "Top Times:" + results[ 0 ].Name + "  " + results[ 0 ].Time;
    }

    /// <summary>
    /// Generates an MD5 hash.
    /// </summary>
    /// <returns>
    /// The hash.
    /// </returns>
    /// <param name='strToEncrypt'>
    /// The string to encrypt.
    /// </param>
    public string Md5Sum(string strToEncrypt)
    {
        //System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        //byte[] bytes = ue.GetBytes(strToEncrypt);
 
        //// encrypt bytes
        //System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //byte[] hashBytes = md5.ComputeHash(bytes);
 
        //// Convert the encrypted bytes back to a string (base 16)
        //string hashString = "";
 
        //for (int i = 0; i < hashBytes.Length; i++)
        //{
        //    hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        //}
 
        //return hashString.PadLeft(32, '0');

        // step 1, calculate MD5 hash from input
        //MD5 md5 = MD5.Create();
        //byte[] inputBytes = Encoding.ASCII.GetBytes( strToEncrypt );
        //byte[] hash = md5.ComputeHash( inputBytes );

        //// step 2, convert byte array to hex string
        //StringBuilder sb = new StringBuilder();
        //for( int i = 0; i < hash.Length; i++ )
        //{
        //    sb.Append( hash[ i ].ToString( "X2" ) );
        //}
        //return sb.ToString();

        string result = "";

        using( MD5 md5 = new MD5CryptoServiceProvider() )
        {
            result = BitConverter.ToString( md5.ComputeHash( ASCIIEncoding.Default.GetBytes( strToEncrypt ) ) );
        }

        return result.Replace( "-", string.Empty );
    }
}

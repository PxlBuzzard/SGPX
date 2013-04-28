using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

using LitJson;

/// <summary>
/// Lap controller.
/// </summary>
/// <author>Pete O'Neal</author>
/// <author>Daniel Jost</author>
public class LapController : MonoBehaviour 
{
    private const string SECRET_KEY = "mySecretKey";
    private const string GET_SCORES_URL = "http://sgpx.coldencullen.com/getscores.php?";
    private const string ADD_SCORE_URL = "http://sgpx.coldencullen.com/addscore.php?";

    public string playerName;
    public string trackName;
    public Timer lapTimer;
	public TextMesh currentLapText;
    public GUIText fastestLapText;
    public GUIText leaderboardText;
    public float fastestTime = 0;
	public Recording fastestRecording;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() 
    {
        lapTimer.LapTimer();
		fastestLapText.text = "";

        UpdateScoreboard();
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		currentLapText.text = lapTimer.currentTime.ToString("f3");

        //UpdateScoreboard();
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
            float lapTime = lapTimer.currentTime;

			//sets your first lap as the fastest
			//or if your current lap is faster then your fastest, make your current lap the new fastest
			if( fastestTime == 0 || fastestTime > lapTime )
			{
				fastestTime = lapTime;
				Debug.Log( "New fastest time: " + fastestTime );
				
				//save the new ghost
				if( collider.transform.parent.GetComponent<Racer>().useVCR )
					fastestRecording = collider.transform.parent.GetComponent<InputVCR>().GetRecording();
			}
			
			//update fastest lap text
			fastestLapText.text = "Fastest Lap: " + fastestTime.ToString("f2");
			lapTimer.Reset();
            lapTimer.LapTimer();

            // Send time to database
            //string hash = Md5Sum( playerName + lapTime + SECRET_KEY );
            string hash = SECRET_KEY;

            WWW result = new WWW(
                ADD_SCORE_URL +
                "name=" + WWW.EscapeURL( playerName ) +
                "&track=" + trackName +
                "&time=" + ( Mathf.Round( lapTime * 1000.0f ) / 1000.0f ) +
                "&hash=" + hash
            );
            //Debug.Log(result.url);
			
			//set up ghost replay
			if( collider.transform.parent.GetComponent<Racer>().useVCR )
			{
				//reset recording
				collider.transform.parent.GetComponent<InputVCR>().NewRecording();
				
				//start fastest time recording
				GameObject.Find( "Ship1Ghost" ).GetComponent<GhostRacer>().StartReplay();
			}

            UpdateScoreboard();
		}
    }

    // Update high scores
    private void UpdateScoreboard()
    {
        // Request data
        WWW bestTimes = new WWW( GET_SCORES_URL + "track=" + trackName );
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

    // Generate MD5 code
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

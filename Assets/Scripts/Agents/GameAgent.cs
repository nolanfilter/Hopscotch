using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameAgent : MonoBehaviour {

	public GameObject backgroundQuadPrefab;
	private BackgroundController backgroundQuadController;

	private GestureAgent.GestureType currentGoalGesture = GestureAgent.GestureType.Invalid;
	private int oldGoalGestureNumber = 0;

	private List<GestureAgent.GestureType> gesturesThisFrame;
	
	private GUIStyle textStyle;
	private Rect textRect;
	private string currentGoalString;

	private static GameAgent mInstance = null;
	public static GameAgent instance
	{
		get
		{
			return mInstance;
		}
	}
	
	void Awake()
	{
		if( mInstance != null )
		{
			Debug.LogError( string.Format( "Only one instance of GameAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;

		gesturesThisFrame = new List<GestureAgent.GestureType>();
	}

	void Start()
	{
		if( backgroundQuadPrefab )
		{
			GameObject go = Instantiate( backgroundQuadPrefab ) as GameObject;
			backgroundQuadController = go.GetComponent<BackgroundController>();
		}

		textStyle = FontAgent.GetTextStyle();
		textRect = new Rect( 0f, 0f, Screen.width, Screen.height );

		StartCoroutine( "ResolveGestures" );

		GetNextGoal();
	}

	void OnEnable()
	{
		GestureAgent.instance.ReportGesture += GestureGet;
	}

	void OnDisable()
	{
		GestureAgent.instance.ReportGesture -= GestureGet;
	}

	void OnGUI()
	{
		GUI.Label( textRect, currentGoalString, textStyle );
	}

	private void GetNextGoal()
	{
		oldGoalGestureNumber = (int)currentGoalGesture;

		currentGoalGesture = (GestureAgent.GestureType)Random.Range( 0, 5 );

		if( (int)currentGoalGesture == oldGoalGestureNumber )
			currentGoalGesture = (GestureAgent.GestureType)( ( oldGoalGestureNumber + 1 )%5 );

		Color backgroundColor = Color.gray;

		switch( currentGoalGesture )
		{
		case GestureAgent.GestureType.Tap: backgroundColor = Color.red; currentGoalString = "tap"; break;
		case GestureAgent.GestureType.TwoFingerTap: backgroundColor = Color.magenta; currentGoalString = "2 finger\ntap"; break;
		case GestureAgent.GestureType.Swipe: backgroundColor = Color.green; currentGoalString = "swipe"; break;
		case GestureAgent.GestureType.TwoFingerSwipe: backgroundColor = Color.blue; currentGoalString = "2 finger\nswipe"; break;
		case GestureAgent.GestureType.Shake: backgroundColor = Color.yellow; currentGoalString = "shake"; break;
		}

		if( backgroundQuadController )
		{
			backgroundQuadController.SetColor( backgroundColor );
			backgroundQuadController.Run();
		}
	}

	private void GestureGet( GestureAgent.GestureType newGesture )
	{
		gesturesThisFrame.Add( newGesture );
	}

	private IEnumerator ResolveGestures()
	{
		while( true )
		{
			yield return new WaitForEndOfFrame();

			if( gesturesThisFrame.Count > 0 )
			{
				if( gesturesThisFrame.Contains( currentGoalGesture ) )
				{
					GetNextGoal();

					yield return new WaitForSeconds( 1f );
				}
				else
				{
					if( backgroundQuadController )
						backgroundQuadController.SetColor( Color.black );
				}

				gesturesThisFrame.Clear();
			}

			yield return new WaitForSeconds( 0.3f );
		}
	}
}

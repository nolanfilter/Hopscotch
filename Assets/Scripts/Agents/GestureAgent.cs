using UnityEngine;
using System.Collections;

public class GestureAgent : MonoBehaviour {

	public delegate void GestureEvent( GestureType newGesture );
	public event GestureEvent ReportGesture;

	public enum GestureType
	{
		Tap = 0,
		TwoFingerTap = 1,
		Swipe = 2,
		TwoFingerSwipe = 3,
		Shake = 4,
		Invalid = 5,
	}

	private static GestureAgent mInstance = null;
	public static GestureAgent instance
	{
		get
		{
			return mInstance;
		}
	}

	// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa). You should be able to use LowPassFilter() function instead of avgSamples()
	private float lowPassKernelWidthInSeconds = 1f;
	
	private float accelerometerUpdateInterval;
	private float lowPassFilterFactor;

	private Vector3 lowPassValue = Vector3.zero;

	private Vector3 acceleration;
	private Vector3 deltaAcceleration;

	private string tapString;
	private string twoFingerTapString;
	private string swipeString;
	private string twoFingerSwipeString;
	private string shakeString;

	private Rect tapRect;
	private Rect twoFingerTapRect;
	private Rect swipeRect;
	private Rect twoFingerSwipeRect;
	private Rect shakeRect;

	void Awake()
	{
		if( mInstance != null )
		{
			Debug.LogError( string.Format( "Only one instance of GestureAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;

		accelerometerUpdateInterval = 1f / 60f;
		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
	}

	void OnEnable()
	{
		FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDoubleTap += FingerGestures_OnFingerDoubleTap;
		FingerGestures.OnFingerSwipe += FingerGestures_OnFingerSwipe;
		FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd; 
        
        FingerGestures.OnTwoFingerLongPress += FingerGestures_OnTwoFingerLongPress;
		FingerGestures.OnTwoFingerTap += FingerGestures_OnTwoFingerTap;
		FingerGestures.OnTwoFingerSwipe += FingerGestures_OnTwoFingerSwipe;
		FingerGestures.OnTwoFingerDragEnd += FingerGestures_OnTwoFingerDragEnd; 
	}
	
	void OnDisable()
	{
		FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDoubleTap -= FingerGestures_OnFingerDoubleTap;
		FingerGestures.OnFingerSwipe -= FingerGestures_OnFingerSwipe;
		FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd;
        
        FingerGestures.OnTwoFingerLongPress -= FingerGestures_OnTwoFingerLongPress;
		FingerGestures.OnTwoFingerTap -= FingerGestures_OnTwoFingerTap;
		FingerGestures.OnTwoFingerSwipe -= FingerGestures_OnTwoFingerSwipe;
		FingerGestures.OnTwoFingerDragEnd -= FingerGestures_OnTwoFingerDragEnd;
    }
	
	void FixedUpdate()
	{
		acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp( lowPassValue, acceleration, lowPassFilterFactor );
		deltaAcceleration = acceleration - lowPassValue;

		//Debug.Log( deltaAcceleration.magnitude );

		if( deltaAcceleration.magnitude > 1.6f )
			ReportGesture( GestureType.Shake );
	}

	private void FingerGestures_OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
	{
		ReportGesture( GestureType.Tap );
	}
	
	private void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos )
	{
		ReportGesture( GestureType.Tap );
	}
	
	private void FingerGestures_OnFingerDoubleTap( int fingerIndex, Vector2 fingerPos )
	{
		ReportGesture( GestureType.Tap );
	}

	private void FingerGestures_OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
	{
		ReportGesture( GestureType.Swipe );
	}
	
	private void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
	{
		ReportGesture( GestureType.Swipe );
    }
    
    private void FingerGestures_OnTwoFingerLongPress( Vector2 fingerPos )
    {
		ReportGesture( GestureType.TwoFingerTap );
    }
    
    private void FingerGestures_OnTwoFingerTap( Vector2 fingerPos )
    {
		ReportGesture( GestureType.TwoFingerTap );
    }
    
    private void FingerGestures_OnTwoFingerSwipe( Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {
		ReportGesture( GestureType.TwoFingerSwipe );
    }
	
	private void FingerGestures_OnTwoFingerDragEnd( Vector2 fingerPos )
	{
		ReportGesture( GestureType.TwoFingerSwipe );
    }
}

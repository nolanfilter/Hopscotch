using UnityEngine;
using System.Collections;

public class CameraAgent : MonoBehaviour {

	private static CameraAgent mInstance = null;
	public static CameraAgent instance
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
			Debug.LogError( string.Format( "Only one instance of CameraAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;
	}

	void Start()
	{
		Camera.main.orthographicSize = Screen.height * 0.5f;
	}
}

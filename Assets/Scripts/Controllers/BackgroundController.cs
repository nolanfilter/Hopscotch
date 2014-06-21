using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

	private float duration = 5f;
	private float beginTime;
	private float currentTime;
	private float lerp;

	private Vector3 beginScale;
	private Vector3 endScale;

	private Vector3 beginPosition;
	private Vector3 endPosition;

	void Start()
	{
		beginScale = transform.localScale;
		endScale = new Vector3( beginScale.x, 0f, beginScale.z );

		beginPosition = Vector3.zero;
		endPosition = new Vector3( 0f, Screen.height * -0.5f, 0f );
	}

	public void Reset()
	{
		transform.localScale = beginScale;
		transform.position = beginPosition;
	}
	public void Run()
	{
		StartCoroutine( "Drain" );
	}

	public void SetColor( Color newColor )
	{
		renderer.material.color = newColor;
	}

	private IEnumerator Drain()
	{
		Reset();

		beginTime = Time.time;

		do
		{
			currentTime = Time.time - beginTime;
			lerp = currentTime / duration;

			transform.localScale = Vector3.Lerp( beginScale, endScale, lerp );
			transform.position = Vector3.Lerp( beginPosition, endPosition, lerp );

			yield return null;

		} while( currentTime < duration );

		transform.localScale = endScale;
		transform.position = endPosition;
	}
}

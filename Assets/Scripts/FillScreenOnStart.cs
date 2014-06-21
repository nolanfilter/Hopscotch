using UnityEngine;
using System.Collections;

public class FillScreenOnStart : MonoBehaviour {

	void Start () {
		transform.localScale = new Vector3( Screen.width, Screen.height, 1f );
	}
}

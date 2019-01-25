using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Api.SendMoveMessage (Vector2.up);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Api.SendMoveMessage (Vector2.down);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Api.SendMoveMessage (Vector2.left);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Api.SendMoveMessage (Vector2.right);
		}
	}
}

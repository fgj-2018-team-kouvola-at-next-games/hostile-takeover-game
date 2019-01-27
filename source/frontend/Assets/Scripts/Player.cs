using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour {

	Block _isNearBlock;
	public Block _isCarrying;
	private System.DateTime lastMoveTimeStamp;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		System.DateTime now = System.DateTime.Now;
		long elapsedMillisecs = (long)((System.TimeSpan)(now - this.lastMoveTimeStamp)).TotalMilliseconds;

		if (lastMoveTimeStamp != null && elapsedMillisecs < 150) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			lastMoveTimeStamp = System.DateTime.Now;
			Api.SendMoveMessage (Vector2.up);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			lastMoveTimeStamp = System.DateTime.Now;
			Api.SendMoveMessage (Vector2.down);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			lastMoveTimeStamp = System.DateTime.Now;
			Api.SendMoveMessage (Vector2.left);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			lastMoveTimeStamp = System.DateTime.Now;
			Api.SendMoveMessage (Vector2.right);
		}

		if (_isCarrying == null && this._isNearBlock != null && Input.GetKeyDown (KeyCode.Space)) {
			Api.SendPickMessage(this._isNearBlock.gameObject.name);
		} else if (_isCarrying != null && Input.GetKeyDown (KeyCode.Space)) {
			Api.SendPlaceMessage ();
        }
	}

	public void SetCarrying(Block target) {
		if (target != null) {
			_isNearBlock = null;
		}

		if (target == null && this._isCarrying != null) {
			this._isCarrying.GetComponentInChildren<ParticleSystem>().Play();
		}

        if (target != null && this._isCarrying == null)
        {
            target.GetComponent<AudioSource>().Play();
        }
        this._isCarrying = target;
	}

	void OnTriggerEnter(Collider collider) {
		if (this._isCarrying != null)
			return;

		Block block = collider.gameObject.GetComponent<Block> ();
		if(!block) return;

		this._isNearBlock = block;
	}

	void OnTriggerExit(Collider collider) {
		Block block = collider.gameObject.GetComponent<Block> ();
		if(!block) return;

		if (this._isNearBlock == block) {
			this._isNearBlock = null;
		}
	}
}

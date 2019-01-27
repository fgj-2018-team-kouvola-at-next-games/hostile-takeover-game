using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour {

	Block _isNearBlock;
	Block _isCarrying;

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

    public void MoveTo(Vector3 position)
    {
        StartCoroutine(_MoveTo(position));
    }

    private IEnumerator _MoveTo(Vector3 targetPos)
    {
        float ANIM_LENGTH = 0.23f;
        float JUMP_HEIGHT = 0.5f;
        float startTime = Time.time;

        Transform t = transform.root;

        Vector3 startPos = t.position;
        if (startPos != targetPos)
        {

            while (Time.time < startTime + ANIM_LENGTH)
            {
                float p = (Time.time - startTime) / ANIM_LENGTH;
                float smoothp = Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, p));

                Vector3 t1 = Vector3.Lerp(startPos, targetPos, p);
                Vector3 t2 = Vector3.Lerp(startPos, targetPos + (Vector3.up * JUMP_HEIGHT * 2), p);

                t.position = Vector3.Lerp(t2, t1, smoothp);
                if (_isCarrying)
                {
                    _isCarrying.transform.position = Vector3.Lerp(t2, t1, smoothp) + Vector3.up;
                }
                yield return new WaitForEndOfFrame();
            }

            if (_isCarrying)
            {
                _isCarrying.transform.position = targetPos + Vector3.up;
            }
            transform.position = targetPos;
        }
    }
}

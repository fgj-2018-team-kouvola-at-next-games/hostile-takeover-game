using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Jumper : MonoBehaviour
{
    public Transform t;
    public Vector3 addPosition = Vector3.zero;
    public float defaultJumpHeight = 0.5f;
    public bool playSoundOnJump = false;

    public void MoveTo(Vector3 position)
    {
        StartCoroutine(_MoveTo(position, this.defaultJumpHeight));
    }

    public void MoveTo(Vector3 position, float jumpHeight)
    {
        StartCoroutine(_MoveTo(position, jumpHeight));
    }

    private IEnumerator _MoveTo(Vector3 movePos, float jumpHeight)
    {
        Vector3 targetPos = movePos + this.addPosition;
        float ANIM_LENGTH = 0.23f;
        float startTime = Time.time;

        Vector3 startPos = t.position;
        if (startPos != targetPos)
        {
            if(this.playSoundOnJump && this.GetComponent<AudioSource>() != null)
            {
                this.GetComponent<AudioSource>().Play();
            }
            while (Time.time < startTime + ANIM_LENGTH)
            {
                float p = (Time.time - startTime) / ANIM_LENGTH;
                float smoothp = Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, p));

                Vector3 t1 = Vector3.Lerp(startPos, targetPos, p);
                Vector3 t2 = Vector3.Lerp(startPos, targetPos + (Vector3.up * jumpHeight * 2), p);

                t.position = Vector3.Lerp(t2, t1, smoothp);
                yield return new WaitForEndOfFrame();
            }
            t.position = targetPos;
        }
    }
}

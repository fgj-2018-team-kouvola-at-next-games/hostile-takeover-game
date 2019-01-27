using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectScaleLooper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<Text>().fontSize = Mathf.RoundToInt(Mathf.Repeat(Time.time, 10)) + 26;
        float newSize = Mathf.PingPong(Time.time * 0.1f, 0.08f) + 1;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(newSize, newSize, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapItem : MonoBehaviour
{


    public void SetColor(Color color)
    {
        this.GetComponent<MeshRenderer>().material.color = color;
    }
}

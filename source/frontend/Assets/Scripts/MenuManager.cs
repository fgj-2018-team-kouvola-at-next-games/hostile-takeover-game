using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    private AppManager appManager;

    void Start() {
        appManager = GameObject.FindObjectOfType<AppManager>();    
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            appManager.StartGame();
        }
    }
}

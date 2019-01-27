using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class AppManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
}

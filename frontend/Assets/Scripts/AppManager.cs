using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class AppManager : MonoBehaviour {

    private SoundManager soundManager;
    public GameObject soundManagerPrefab;

    // Start is called before the first frame update
    void Start() {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (!soundManager) {
            GameObject smObject = Instantiate(soundManagerPrefab);
            soundManager = smObject.GetComponent<SoundManager>();
        }
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
}

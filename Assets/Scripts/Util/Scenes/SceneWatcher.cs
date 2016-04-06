using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneWatcher : MonoBehaviour {
    private static bool started = false;

    void Start() {
        if (!started) {
            if (SceneManager.GetActiveScene().buildIndex != (int)Scene.SplashScreen) {
                SceneManager.LoadScene((int)Scene.SplashScreen);
            }

            started = true;
        }
    }
}

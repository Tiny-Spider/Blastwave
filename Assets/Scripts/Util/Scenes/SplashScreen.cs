using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour {
    public float timeBeforeFadeOut;

    void Start() {
        StartCoroutine(StartFade());
    }

    IEnumerator StartFade() {
        yield return new WaitForSeconds(timeBeforeFadeOut);
        SceneManager.LoadScene((int)Scene.Menu);
    }
}

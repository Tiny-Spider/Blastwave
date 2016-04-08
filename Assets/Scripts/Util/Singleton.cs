using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;

    public static T GetInstance() {
        if (instance == null) {
            instance = FindObjectOfType<T>();

            if (instance == null) {
                Debug.LogError("Missing reference to \"" + typeof(T).ToString() + "\" please add this to SplashScreen!");
            }
        }

        return instance;
    }
}

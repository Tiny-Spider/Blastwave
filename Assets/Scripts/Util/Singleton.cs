using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private const string mTag = "GameManager";
    private static T instance;

    public static T GetInstance() {
        if (instance == null) {
            instance = FindObjectOfType<T>();

            if (instance == null) {
                GameObject gameManagerObject = GameObject.FindGameObjectWithTag(mTag);

                if (gameManagerObject) {
                    instance = gameManagerObject.AddComponent<T>();
                }
                else {
                    Debug.LogError("A GameObject with the tag " + mTag + " is needed in the scene, but there is none.");
                }
            }
        }

        return instance;
    }
}

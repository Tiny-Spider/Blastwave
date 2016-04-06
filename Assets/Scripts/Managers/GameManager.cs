using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
    public PlayMode playMode;
    public LevelData level;
    public User[] users;
    public Color[] userColors;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded(int scene) {
        Debug.Log("Scene: " + scene + " was loaded!");
    }
}

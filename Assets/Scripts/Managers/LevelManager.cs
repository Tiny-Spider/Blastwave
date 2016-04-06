using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    private static LevelData[] levels = new LevelData[0];

    void Start() {
        levels = Resources.LoadAll<LevelData>("Levels");

        if (levels.Length <= 0) {
            Debug.LogError("No levels were loaded!");
        }
    }

    public static LevelData[] GetLevels() {
        return levels;
    }

    public static LevelData GetLevel(string displayName) {
        foreach (LevelData level in levels) {
            if (level.displayName.Equals(displayName)) {
                return level;
            }
        }

        return null;
    }
}
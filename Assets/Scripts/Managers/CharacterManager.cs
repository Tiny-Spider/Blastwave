using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {
    private static CharacterData[] characters = new CharacterData[0];
    private static int maxIndex = 0;

    void Start() {
        characters = Resources.LoadAll<CharacterData>("Characters");
        maxIndex = characters.Length - 1;

        if (characters.Length <= 0) {
            Debug.LogError("No characters were loaded!");
        }
    }

    public static CharacterData[] GetCharacters() {
        return characters;
    }

    public static CharacterData GetCharacter(int index) {
        return characters[index];
    }

    public static int GetMaxIndex() {
        return maxIndex;
    }
}
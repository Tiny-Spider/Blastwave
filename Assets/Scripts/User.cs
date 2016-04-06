using UnityEngine;
using System.Collections;

public class User {
    public InputDevice input;
    public int colorIndex;
    public int characterIndex;

    public User(InputDevice input, int colorIndex) {
        this.input = input;
        this.colorIndex = colorIndex;
        this.characterIndex = 0;
    }

    public CharacterData GetCharacter() {
        return CharacterManager.GetCharacters()[characterIndex];
    }

    public Color GetColor() {
        return GameManager.GetInstance().userColors[colorIndex];
    }
}

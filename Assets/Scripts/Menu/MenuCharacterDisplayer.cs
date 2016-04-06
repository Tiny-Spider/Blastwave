using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCharacterDisplayer : MonoBehaviour {
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text description;

    public void SetCharacter(CharacterData character) {
        image.sprite = character.displayImage;
        title.text = character.displayName;
        description.text = character.description;
    }
}
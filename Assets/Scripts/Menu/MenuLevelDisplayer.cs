using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuLevelDisplayer : MonoBehaviour {
    [SerializeField]
    private Image levelImage;
    [SerializeField]
    private Text descriptionText;

    public void SetLevel(LevelData level) {
        levelImage.sprite = level.displayImage;
        descriptionText.text = level.description;
    }
}
using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/Character")]
public class CharacterData : ScriptableObject {
    public Player prefab;
    public Sprite displayImage;
    public string displayName;
    [MultilineAttribute]
    public string description;
}
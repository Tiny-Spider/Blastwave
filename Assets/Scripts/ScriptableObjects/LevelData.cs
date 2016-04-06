using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
public class LevelData : ScriptableObject {
    public int sceneId;
    public Sprite displayImage;
    public string displayName;
    [MultilineAttribute]
    public string description;
}
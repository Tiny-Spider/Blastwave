using UnityEngine;
using System.Collections;

public abstract class LivingEntityData<T> : ScriptableObject where T : LivingEntity {
    public T prefab;

    [Header("Base Values")]
    public int health = 10;
    public int maxHealth = 10;
    public float moveSpeed = 5.0f;

    [Header("Display Values")]
    public Name names;
    public Sprite displayImage;
    [MultilineAttribute]
    public string description;

    public abstract T Initalize();

    public LivingEntityData<LivingEntity> GetBase() {
        return this as LivingEntityData<LivingEntity>;
    }
}

[System.Serializable]
public struct Name {
    public string displayName;
    public string techName;
}

using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemyData : LivingEntityData<Enemy> {

    public override Enemy Initalize() {
        Enemy enemy = GameObject.Instantiate<Enemy>(prefab);
        enemy.Initalize(this);

        return enemy;
    }
}

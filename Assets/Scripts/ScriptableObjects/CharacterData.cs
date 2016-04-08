using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/Character")]
public class CharacterData : LivingEntityData<Player> {

    public override Player Initalize() {
        Player player = GameObject.Instantiate<Player>(prefab);
        player.Initalize(this);

        return player;
    }
}
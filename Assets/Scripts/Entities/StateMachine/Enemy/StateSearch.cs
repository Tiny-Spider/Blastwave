using UnityEngine;
using System.Collections;

public class StateSearch : State<Enemy> {
    public static readonly StateSearch instance = new StateSearch();

    public override void Enter(Enemy enemy) {
        if (enemy.debug) {
            Debug.Log("[State] Enemy \"" + enemy.names.techName + "\" switched to \"StateSearch\"");
        }
    }

    public override void Execute(Enemy enemy) {
        Level level = enemy.level;

        Vector3 myPosition = enemy.transform.position;
        Player targetPlayer = null;
        float targetDistance = 0;

        foreach (Player player in level.players) {
            if (player == null || player.dead) {
                continue;
            }

            if (targetPlayer == null) {
                targetPlayer = player;
                targetDistance = Vector3.Distance(myPosition, targetPlayer.transform.position);
                continue;
            }

            float distance = Vector3.Distance(myPosition, player.transform.position);

            if (distance < targetDistance) {
                targetPlayer = player;
                targetDistance = distance;
            }
        }

        if (!targetPlayer) {
            enemy.Dance();
            return;
        }

        enemy.target = targetPlayer;
        enemy.SwitchToState(StateChase.instance);
    }

    public override void Exit(Enemy enemy) {

    }
}

using UnityEngine;
using System.Collections;

public class StateMove : State<Enemy> {
    private static readonly StateMove _instance = new StateMove();
    public static StateMove instance {
        get {
            return _instance;
        }
    }

    public override void Enter(Enemy enemy) {
        enemy.pathfinder.updateRotation = true;

        if (enemy.debug) {
            Debug.Log("[State] Enemy \"" + enemy.names.techName + "\" switched to \"StateMove\"");
        }

        //unit.navMeshAgent.SetDestination(unit.targetPosition);
    }

    public override void Execute(Enemy enemy) {
        NavMeshAgent pathfinder = enemy.pathfinder;

        if (!pathfinder.pathPending) {
            if (pathfinder.remainingDistance <= pathfinder.stoppingDistance) {
                if (!pathfinder.hasPath || pathfinder.velocity.sqrMagnitude == 0f) {
                    enemy.SwitchToState(StateSearch.instance);
                    return;
                }
            }
        }
    }

    public override void Exit(Enemy enemy) {
        enemy.pathfinder.ResetPath();
    }
}

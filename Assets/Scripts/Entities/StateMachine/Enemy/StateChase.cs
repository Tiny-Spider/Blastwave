using UnityEngine;
using System.Collections;

public class StateChase : State<Enemy> {
    public static readonly StateChase instance = new StateChase();

    private int currentFrame = -1;
    private float searchCooldown;
    private float moveCooldown;

    private bool shouldSearch = false;
    private bool shouldMove = false;

    public override void Enter(Enemy enemy) {
        enemy.pathfinder.updateRotation = true;

        if (enemy.debug) {
            Debug.Log("[State] Enemy \"" + enemy.names.techName + "\" switched to \"StateChase\"");
        }
    }

    public override void Execute(Enemy enemy) {
        if (enemy.target == null || enemy.target.IsDead()) {
            enemy.SwitchToState(StateSearch.instance);
            return;
        }

        Vector3 myPosition = enemy.transform.position;
        Vector3 targetPosition = enemy.target.transform.position;
        float targetDistance = Vector3.Distance(myPosition, targetPosition);

        if (enemy.debug) {
            Debug.Log("[StateChase] Enemy \"" + enemy.names.techName + "\" targetDistance: " + targetDistance);
        }

        // Try and attack with a weapon, if any weapon can attack switch to attack state
        foreach (Weapon weapon in enemy.GetWeapons()) {
            if (targetDistance <= weapon.range) {
                RaycastHit hit;

                // Check for line of sight
                Ray ray = new Ray(myPosition, targetPosition - myPosition);

                if (enemy.debug) {
                    Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
                }

                if (Physics.Raycast(ray, out hit, targetDistance, enemy.targetMask, QueryTriggerInteraction.Collide)) {
                    if (hit.collider.transform.Equals(enemy.target.transform)) {
                        enemy.SwitchToState(StateAttack.instance);
                        return;
                    }
                }
            }
        }

        // If first call this frame, then do the checks
        /*
        if (currentFrame != Time.frameCount) {
            currentFrame = Time.frameCount;

            shouldMove = Time.time > moveCooldown;
            shouldSearch = Time.time > searchCooldown;

            // Can only do one of each
            if (shouldSearch) {
                searchCooldown = Time.time + enemy.searchInterval;
            }
            else if (shouldMove) {
                moveCooldown = Time.time + enemy.movementUpdateSpeed;
            }
        }
        */

        // Can only do one of each
        enemy.MoveTo(targetPosition);
        /*
        if (shouldSearch) {
            enemy.SwitchToState(StateSearch.instance);
        }
        else if (shouldMove) {
            enemy.MoveTo(targetPosition);
        }
        */
    }

    public override void Exit(Enemy enemy) {
        enemy.pathfinder.ResetPath();
    }
}

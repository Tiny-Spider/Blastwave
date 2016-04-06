using UnityEngine;
using System.Collections;

public class StateAttack : State<Enemy> {
    public static readonly StateAttack instance = new StateAttack();

    public override void Enter(Enemy enemy) {
        enemy.pathfinder.updateRotation = false;

        if (enemy.debug) {
            Debug.Log("[State] Enemy \"" + enemy.names.techName + "\" switched to \"StateAttack\"");
         }
    }

    public override void Execute(Enemy enemy) {
        // No target found, go search for one
        if (enemy.target == null || enemy.target.IsDead()) {
            enemy.SwitchToState(StateSearch.instance);
            return;
        }

        Vector3 myPosition = enemy.transform.position;
        Vector3 targetPosition = enemy.target.transform.position;
        float targetDistance = Vector3.Distance(myPosition, targetPosition);

        if (enemy.debug) {
            Debug.Log("[StateAttack] Enemy \"" + enemy.names.techName + "\" targetDistance: " + targetDistance);
        }

        // Try and attack with a weapon, if any weapon can attack it will stay in this state
        foreach (Weapon weapon in enemy.GetWeapons()) {
            if (!weapon) {
                continue;
            }

            if (targetDistance <= weapon.range) {
                RaycastHit hit;
                Ray ray = new Ray(myPosition, targetPosition - myPosition);

                if (enemy.debug) {
                    Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
                }

                if (Physics.Raycast(ray, out hit, targetDistance, enemy.targetMask, QueryTriggerInteraction.Collide)) {
                    if (hit.collider.transform.Equals(enemy.target.transform)) {
                        // Looking
                        Quaternion targetRotation = Quaternion.LookRotation((enemy.target.transform.position + enemy.target.velocity * .5f) - enemy.transform.position);
                        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.lookSmoothing * Time.fixedDeltaTime);
                        
                        enemy.Attack();
                        return;
                    }
                }
            }
        }

        // Can't attack with any weapon, go back to chasing
        enemy.SwitchToState(StateChase.instance);
    }

    public override void Exit(Enemy enemy) {
        enemy.pathfinder.updateRotation = true;
    }
}

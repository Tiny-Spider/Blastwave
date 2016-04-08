using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    [Header("Weapon Settings")]
    public float range;
    public int damage;
    public float cooldown;

    protected LayerMask targetMask;
    protected float nextCooldown;

    public void Initalize(LayerMask targetMask) {
        this.targetMask = targetMask;
    }

    public virtual void Attack(LivingEntity attacker) {
        if (CheckCooldown()) {
            Ray ray = new Ray(attacker.transform.position, attacker.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range, targetMask, QueryTriggerInteraction.Collide)) {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();

                if (damageable != null) {
                    damageable.Damage(attacker, damage, hit.point, transform.forward);
                }
            }
        }
    }

    // A method to check if cooldown is over and apply a new cooldown if so
    protected virtual bool CheckCooldown() {
        if (Time.time > nextCooldown) {
            nextCooldown = Time.time + cooldown;
            return true;
        }

        return false;
    }
}

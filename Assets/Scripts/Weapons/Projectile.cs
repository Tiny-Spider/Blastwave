using UnityEngine;
using System.Collections;

// Generic projectile class, can be used by the Gun class
public class Projectile : MonoBehaviour {
    public float speed;
    [Range(0, 1), Tooltip("Distance to check extra to stop moving trough objects")]
    public float predictionDistance = 0.1f;

    public TrailRenderer[] trailRenders;

    private int damage;
    private IDamageSource source;
    private LayerMask targetMask;

    void OnEnable() {
        for (int i = 0; i < trailRenders.Length; i++) {
            trailRenders[i].Clear();
        }

        // Check to see if we've spawned in a wall or enemy
        Collider[] initalCollisions = Physics.OverlapSphere(transform.position, predictionDistance, targetMask, QueryTriggerInteraction.Collide);
        if (initalCollisions.Length > 0) {
            OnCollide(initalCollisions[0], transform.position);
        }
    }

    void OnDisable() {
        CancelInvoke("Die");

        for (int i = 0; i < trailRenders.Length; i++) {
            trailRenders[i].Clear();
        }
    }

    public void Initalize(IDamageSource source, int damage, float distance, LayerMask targetMask) {
        this.damage = damage;
        this.source = source;
        this.targetMask = targetMask;

        float destroyTime = distance / speed;
        Invoke("Die", destroyTime);
    }

    void Update() {
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollision(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + predictionDistance, targetMask, QueryTriggerInteraction.Collide)) {
            OnCollide(hit.collider, hit.point);
        }
    }

    void OnCollide(Collider collider, Vector3 point) {
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null) {
            damageable.Damage(source, damage, point, transform.forward);
        }

        Die();
    }

    void Die() {
        gameObject.Recycle();
    }
}

using UnityEngine;
using System.Collections;

public class Gun : Weapon {
    [Header("Muzzle")]
    public Transform muzzle;
    public Transform muzzleFlash;
    public LineRenderer muzzleLine;
    public float muzzleFlashTime = 0.1f;

    [Header("Projectile")]
    public int initalPoolSize = 20;
    [Tooltip("Set to null for an instant bullet instead")]
    public Projectile projectile;

    [Header("Weapon Settings")]
    public int reloadTime;
    public int magazineSize;
    public float fireRate;

    [Header("Laser")]
    public bool laserEnabled = true;
    public float laserDistance;
    public LineRenderer laserRenderer;

    [Header("Debug")]
    public Color debugColor = Color.red;
    public float debugDistance = 1f;

    int magazine;

    void Awake() {
        magazine = magazineSize;
        cooldown = 1.0f / fireRate;
        laserRenderer.enabled = laserEnabled;

        // Cleanup
        muzzleLine.enabled = false;
        muzzleFlash.gameObject.SetActive(false);

        // Pooling
        projectile.CreatePool(initalPoolSize);
    }

    void Update() {
        if (laserEnabled && laserRenderer) {
            RaycastHit hit;

            laserRenderer.SetPosition(0, muzzle.position);

            if (Physics.Raycast(muzzle.position, transform.forward, out hit, laserDistance, targetMask, QueryTriggerInteraction.Collide)) {
                laserRenderer.SetPosition(1, hit.point);
            }
            else {
                laserRenderer.SetPosition(1, muzzle.position + (muzzle.forward * laserDistance));
            }
        }
    }

    public override void Attack(LivingEntity attacker) {
        if (CheckCooldown()) {
            // Skip whole magazine stuff if the the size is set to infinite (-1)
            if (magazineSize <= 0) {
                Shoot();
                return;
            }
            
            // Magazine code
            if (magazine > 0) {
                magazine--;
                Shoot();
            } else {
                Reload();
            }
        }
    }

    public void Shoot() {
        StartCoroutine(MuzzleFlash());

        if (projectile) {
            Projectile newProjectile = projectile.Spawn(muzzle.position, muzzle.rotation);
            newProjectile.Initalize(damage, range, targetMask);
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, targetMask, QueryTriggerInteraction.Collide)) {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null) {
                damageable.Damage(damage, hit.point, transform.forward);
            }
        }

        if (hit.collider == null) {
            StartCoroutine(BulletLine(muzzle.position + (muzzle.forward * range)));
        }
        else {
            StartCoroutine(BulletLine(hit.point));
        }
    }

    IEnumerator MuzzleFlash() {
        muzzleFlash.gameObject.SetActive(true);

        yield return new WaitForSeconds(muzzleFlashTime);

        muzzleFlash.gameObject.SetActive(false);
    }

    IEnumerator BulletLine(Vector3 targetPosition) {
        muzzleLine.SetPosition(0, muzzle.position);
        muzzleLine.SetPosition(1, targetPosition);
        muzzleLine.enabled = true;

        yield return new WaitForSeconds(muzzleFlashTime);

        muzzleLine.enabled = false;
    }

    public void Reload() {
        nextCooldown = Time.time + reloadTime;
        magazine = magazineSize;
    }

    void OnDrawGizmos() {
        Gizmos.color = debugColor;
        Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * debugDistance);
    }
}

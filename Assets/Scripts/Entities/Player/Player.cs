using UnityEngine;
using System.Collections;

public class Player : LivingEntity {
    private User user;

    [Header("Rendering")]
    public Renderer colorRenderer;

    [Header("Movement")]
    public Vector3 velocity;
    public float moveSpeed = 5.0f;

    [Header("Weapons")]
    public LayerMask targetMask;
    public Transform[] hands;
    public Weapon[] weapons;

    public delegate void PlayerDamageEvent(Player player, int damage, int health, int healthRemaining);
    public static event PlayerDamageEvent OnPlayerDamage = delegate { };

    public delegate void PlayerDeathEvent(Player player);
    public static event PlayerDeathEvent OnPlayerDeath = delegate { };

    public void Initalize(User user) {
        this.user = user;

        colorRenderer.material.color = user.GetColor();

        foreach (Weapon weapon in weapons) {
            weapon.Initalize(targetMask);
        }
    }

    public User GetUser() {
        return user;
    }

    public void Revive() {
        gameObject.SetActive(true);
        dead = false;
    }

    public override void Damage(int damage, Vector3 hitPoint, Vector3 hitDirection) {
        base.Damage(damage, hitPoint, hitDirection);

        OnPlayerDamage(this, damage, health + damage, health);
    }

    public override void Die() {
        gameObject.SetActive(false);
        dead = true;

        OnPlayerDeath(this);
    }
}

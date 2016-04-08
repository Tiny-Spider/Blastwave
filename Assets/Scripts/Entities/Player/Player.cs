using UnityEngine;
using System.Collections;

public class Player : LivingEntity {
    private User user;

    [Header("Rendering")]
    public Renderer colorRenderer;

    [Header("Movement")]
    public Vector3 velocity;

    [Header("Weapons")]
    public LayerMask targetMask;
    public Transform[] hands;
    public Weapon[] weapons;

    public delegate void PlayerDamageEvent(Player player, IDamageSource source, int damage, int health, int healthRemaining);
    public static event PlayerDamageEvent OnPlayerDamage = delegate { };

    public delegate void PlayerDeathEvent(Player player, IDamageSource source);
    public static event PlayerDeathEvent OnPlayerDeath = delegate { };

    public void Initalize(User user) {
        this.user = user;

        colorRenderer.material.color = user.GetColor();

        foreach (Weapon weapon in weapons) {
            weapon.Initalize(targetMask);
        }
    }

    public void Initalize(CharacterData data) {
        base.Initalize(data.GetBase());
    }

    public User GetUser() {
        return user;
    }

    public void Revive() {
        gameObject.SetActive(true);
        dead = false;
    }

    public override void Damage(IDamageSource source, int damage, Vector3 hitPoint, Vector3 hitDirection) {
        base.Damage(source, damage, hitPoint, hitDirection);

        OnPlayerDamage(this, source, damage, health + damage, health);
    }

    public override void Die(IDamageSource source) {
        gameObject.SetActive(false);
        dead = true;

        OnPlayerDeath(this, source);
    }
}

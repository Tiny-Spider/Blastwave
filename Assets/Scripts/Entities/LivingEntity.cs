using UnityEngine;
using System.Collections;

public abstract class LivingEntity : MonoBehaviour, IDamageable, IDamageSource {
    public bool debug = false;

    public int health { protected set; get; }
    public int maxHealth { protected set; get; }
    public float moveSpeed { protected set; get; }
    public Name names { protected set; get; }
    public bool dead { protected set; get; }

    public void Initalize(LivingEntityData<LivingEntity> data) {
        health = data.health;
        maxHealth = data.maxHealth;
        moveSpeed = data.moveSpeed;
        names = data.names;

        dead = health <= 0;
    }

    public virtual void Damage(IDamageSource source, int damage, Vector3 hitPoint, Vector3 hitDirection) {
        if (debug) {
            Debug.Log("[Damage] LivingEntity \"" + names.techName + "\" was attacked by \"" + source.names.techName + "\" and now has \"(" + health + " - " + damage + ")\" = " + (health - damage) + " health");
        }

        health -= damage;

        if (health <= 0) {
            Die(source);
        }
    }

    public virtual void Heal(int amount) {
        if (debug) {
            Debug.Log("[Heal] LivingEntity \"" + names.techName + "\" was healed and now has \"(" + health + " + " + amount + ")\" = " + (health + amount) + " health");
        }

        health += amount;

        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    public abstract void Die(IDamageSource source);
}

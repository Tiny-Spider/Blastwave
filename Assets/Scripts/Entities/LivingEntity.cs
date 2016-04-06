using UnityEngine;
using System.Collections;

public abstract class LivingEntity : MonoBehaviour, IDamageable {
    public bool debug = false;

    [Header("LivingEntity")]
    public int health = 10;
    public int maxHealth = 10;
    public Name names;

    protected bool dead = false;

    public virtual void Damage(int damage, Vector3 hitPoint, Vector3 hitDirection) {
        if (debug) {
            Debug.Log("[Damage] LivingEntity \"" + names.techName + "\" now has \"(" + health + " - " + damage + ")\" = " + (health - damage) + " health");
        }

        health -= damage;

        if (health <= 0) {
            Die();
        }
    }

    public bool IsDead() {
        return dead;
    }

    public virtual void Heal(int amount) {
        health += amount;

        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    public abstract void Die();

    [System.Serializable]
    public struct Name {
        public string displayName;
        public string techName;
    }
}

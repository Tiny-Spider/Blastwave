using UnityEngine;
using System.Collections;

// Make this an interface so non-living objects can also take damage
public interface IDamageable {
    void Damage(int damage, Vector3 hitPoint, Vector3 hitDirection);
}

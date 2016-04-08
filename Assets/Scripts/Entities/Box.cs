﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour, IDamageable {
    public float hitForce;

    private Rigidbody myRigidbody;

    void Awake() {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Damage(IDamageSource source, int damage, Vector3 hitPoint, Vector3 hitDirection) {
        myRigidbody.AddForceAtPosition(hitDirection * hitForce, hitPoint, ForceMode.Impulse);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour {
    public float smoothing = 8;
    public float distanceDevide = 2;
    public Vector3 centerOffset;
    public Vector3 defaultCenter = Vector3.zero;

    private List<LivingEntity> targetEntities = new List<LivingEntity>();
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetCameraPosition = Vector3.zero;
    private float minDistance;
    private float distance;
    private Camera targetCamera;

    void Awake() {
        targetCamera = GetComponentInChildren<Camera>();
        targetCameraPosition = targetCamera.transform.localPosition;
        minDistance = targetCameraPosition.y;
    }

    void FixedUpdate() {
        CalculateCenter();
        CalculateFov();

        float deltaTime = Time.deltaTime * smoothing;
        transform.position = Vector3.Lerp(transform.position, targetPosition, deltaTime);
        targetCamera.transform.localPosition = Vector3.Lerp(targetCamera.transform.localPosition, targetCameraPosition, deltaTime);
    }

    public void SetTargets(IEnumerable<LivingEntity> livingEntities) {
        foreach (LivingEntity livingEntity in livingEntities) {
            targetEntities.Add(livingEntity);
        }
    }

    void CalculateCenter() {
        targetPosition = defaultCenter;

        if (targetEntities.Count > 0) {
            int numPoints = 0;

            foreach (LivingEntity livingEntity in targetEntities) {
                if (livingEntity && livingEntity.gameObject.activeInHierarchy && !livingEntity.IsDead()) {
                    targetPosition += livingEntity.transform.position;
                    numPoints++;
                }
            }

            if (numPoints > 0) {
                targetPosition /= numPoints;
                distance = Vector3.Distance(targetPosition, targetEntities[0].transform.position);
            }
        }
    }

    void CalculateFov() {
        targetPosition += centerOffset * Mathf.Max(0, distance);
        targetCameraPosition.y = Mathf.Max(minDistance, distance / distanceDevide);
    }
}
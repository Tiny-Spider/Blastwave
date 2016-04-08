using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    public float warmupDelay = 2f;
    public float spawnDelay = 1f;

    public EnemyData[] enemies;
    public Transform[] spawnPoints;

    public void StartWaves() {

    }

    public void StartWave(float spawnDelay) {
        StartCoroutine("Waves", spawnDelay);
    }

    public void StopWave() {
        StopCoroutine("Waves");
    }

    IEnumerator Waves(float spawnDelay) {
        WaitForSeconds delay = new WaitForSeconds(spawnDelay);

        while (true) {
            yield return delay;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Enemy enemy = enemies[Random.Range(0, enemies.Length)].Spawn(spawnPoint.position);
        }
    }

    private void Clear() {
        //foreach (Enemy enemy in enemies) {
        //    enemy.RecycleAll();
        //}
    }
}

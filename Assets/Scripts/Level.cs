using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    public GameCamera userCamera;
    public WaveManager waveManager;
    public Transform[] spawnPoints;

    [HideInInspector]
    public List<Player> players;

    private GameManager gameManager;

    void Awake() {
        gameManager = GameManager.GetInstance();
    }

    void Start() {
        // Temp list of spawnpoints
        List<Transform> spawnPoints = new List<Transform>(this.spawnPoints);

        foreach (User user in gameManager.users) {
            if (user != null) {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                Player player = user.GetCharacter().Initalize();

                player.Initalize(user);
                player.transform.position = spawnPoint.position;

                players.Add(player);
                spawnPoints.Remove(spawnPoint);
            }
        }
        
        // Puke (Unity pls update to C# 6)
        userCamera.SetTargets(players.ConvertAll<LivingEntity>(x => (LivingEntity)x));
        //userCamera.SetTargets(players);

        waveManager.StartWaves();
    }
}

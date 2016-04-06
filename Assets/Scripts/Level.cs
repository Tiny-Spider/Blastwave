using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    public GameCamera userCamera;
    public Transform[] spawnPoints;
    [HideInInspector]
    public List<Player> players;
    public WaveManager waveManager;

    private GameManager gameManager;

    void Awake() {
        gameManager = GameManager.GetInstance();
    }

    void Start() {
        List<Transform> spawnPoints = new List<Transform>(this.spawnPoints);

        foreach (User user in gameManager.users) {
            if (user != null) {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                CharacterData character = CharacterManager.GetCharacter(user.characterIndex);
                Player player = GameObject.Instantiate<Player>(character.prefab);

                player.Initalize(user);
                player.transform.position = spawnPoint.position;

                players.Add(player);
                spawnPoints.Remove(spawnPoint);
            }
        }
        
        // Puke // (Unity pls update to .NET 4)
        userCamera.SetTargets(players.ConvertAll<LivingEntity>(x => (LivingEntity)x));
        //userCamera.SetTargets(players);

        StartCoroutine(Waves());
    }

    IEnumerator Waves() {
        yield return new WaitForSeconds(2f);

        waveManager.StartWave(1f);
    }
}

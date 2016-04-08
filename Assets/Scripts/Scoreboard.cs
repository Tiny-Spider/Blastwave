using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {
    private Dictionary<User, int> scores = new Dictionary<User, int>();

    void Start() {
        Enemy.OnEnemyDeath += OnEnemyDeath;
    }

    void OnEnemyDeath(Enemy enemy, IDamageSource source) {

    }

    public void AddScore(User user, int score) {
        
    }

    public struct Score {
        public int score;
        public Dictionary<Name, int> kills;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(MenuControl))]
public class HUDManager : MonoBehaviour {
    public MenuPanel deathPanel;
    public MenuPanel loadingPanel;

    private Level level;
    private MenuControl menuControl;

    void Awake() {
        level = FindObjectOfType<Level>();
        menuControl = GetComponent<MenuControl>();
    }

    void Start() {
        Player.OnPlayerDeath += OnPlayerDeath;
    }

    void OnPlayerDeath(Player player) {
        foreach (Player _player in level.players) {
            if (!_player.IsDead()) {
                return;
            }
        }

        menuControl.ShowPanel(deathPanel);
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        menuControl.ShowPanel(loadingPanel);
        SceneManager.LoadScene((int)Scene.Menu);
    }
}

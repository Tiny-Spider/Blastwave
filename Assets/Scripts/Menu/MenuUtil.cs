using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MenuControl))]
public class MenuUtil : MonoBehaviour {
    [Header("Panels")]
    public MenuPanel singleplayerPanel;
    public MenuPanel cooperativePanel;
    public MenuPanel loadingPanel;
    [Header("Setup")]
    public Dropdown gamemodeDropdown;
    public Dropdown levelDropdown;
    public MenuLevelDisplayer levelDisplayer;

    private GameManager gameManager;
    private MenuControl menuControl;

    void Awake() {
        gameManager = GameManager.GetInstance();
        menuControl = GetComponent<MenuControl>();
    }

    void Start() {
        LevelData[] levels = LevelManager.GetLevels();
        List<string> levelList = new List<string>();

        foreach (LevelData level in levels) {
            levelList.Add(level.displayName);
        }

        levelDropdown.ClearOptions();
        levelDropdown.AddOptions(levelList);
        levelDropdown.onValueChanged.Invoke(0);
    }

    // Setters
    public void SetLevel(int index) {
        LevelData level = LevelManager.GetLevel(levelDropdown.options[index].text);

        if (level != null) {
            gameManager.level = level;
            levelDisplayer.SetLevel(level);
        }
    }

    public void SetPlayMode(int playMode) {
        gameManager.playMode = (PlayMode)playMode;
    }

    public void SetSinglePlayerInput(int inputController) {
        gameManager.users[0].input = (InputDevice)inputController;
    }
    // End setters

    public void StartGame() {
        menuControl.ShowPanel(loadingPanel);
        SceneManager.LoadScene(gameManager.level.sceneId);
    }

    public void ShowStartPanel() {
        switch (gameManager.playMode) {
            case PlayMode.SinglePlayer:
                gameManager.users = new User[1];
                menuControl.ShowPanel(singleplayerPanel);
                break;
            case PlayMode.Cooperative:
                gameManager.users = new User[4];
                menuControl.ShowPanel(cooperativePanel);
                break;
            default:
                break;
        }
    }

    public void Exit() {
#if UNITY_EDITOR
        Debug.Break();
#else
        Application.Quit();
#endif
    }
}

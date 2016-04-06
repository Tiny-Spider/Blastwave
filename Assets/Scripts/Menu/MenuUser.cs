using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuUser : MonoBehaviour {
    private User user;
    private MenuUserManager menuUserManager;

    public MenuCharacterDisplayer characterDisplayer;

    public Text inputText;
    public Image background;
    public GameObject joinPanel;
    public GameObject selectionPanel;

    void Awake() {
        menuUserManager = FindObjectOfType<MenuUserManager>();

        SetHasUser(false);
    }

    void Update() {
        if (HasUser()) {
            if (InputManager.GetButtonDown(user.input, InputButton.Left)) {
                user.colorIndex = menuUserManager.GetNextColor(user.colorIndex, false);

                UpdatePanel();
            }
            else if (InputManager.GetButtonDown(user.input, InputButton.Right)) {
                user.colorIndex = menuUserManager.GetNextColor(user.colorIndex, true);

                UpdatePanel();
            }

            if (InputManager.GetButtonDown(user.input, InputButton.ChooseLeft)) {
                user.characterIndex = user.characterIndex < CharacterManager.GetMaxIndex() ? user.characterIndex + 1 : 0;

                UpdatePanel();
            }
            else if (InputManager.GetButtonDown(user.input, InputButton.ChooseRight)) {
                user.characterIndex = user.characterIndex > 0 ? user.characterIndex - 1 : CharacterManager.GetMaxIndex();

                UpdatePanel();
            }
        }
    }

    public void SetUser(User user) {
        this.user = user;

        SetHasUser(true);

        UpdatePanel();
    }

    public void RemoveUser() {
        this.user = null;

        SetHasUser(false);
    }

    public bool HasUser() {
        return user != null;
    }

    public User GetUser() {
        return user;
    }

    public void UpdatePanel() {
        inputText.text = user.input.ToString();
        characterDisplayer.SetCharacter(user.GetCharacter());
        background.color = user.GetColor();
    }

    private void SetHasUser(bool hasUser) {
        joinPanel.SetActive(!hasUser);
        selectionPanel.SetActive(hasUser);
    }
}
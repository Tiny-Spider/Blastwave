using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuUserManager : MonoBehaviour {
    public MenuUser[] menuUsers;

    private GameManager gameManager;
    private MenuUtil menuUtil;

    // InputDevice | Used
    private Dictionary<InputDevice, bool> inputDevices = new Dictionary<InputDevice, bool>() {
        { InputDevice.Keyboard, false },
        { InputDevice.Controller1, false },
        { InputDevice.Controller2, false },
        { InputDevice.Controller3, false },
        { InputDevice.Controller4, false }
    };

    private bool[] colorsUse;

    void Awake() {
        gameManager = GameManager.GetInstance();
        menuUtil = FindObjectOfType<MenuUtil>();

        if (gameManager) {
            colorsUse = new bool[gameManager.userColors.Length];
        }
    }

    void Update() {
        for (int i = 0; i < menuUsers.Length; i++) {
            MenuUser menuUser = menuUsers[i];
            User user = menuUser.GetUser();

            if (user == null) {
                foreach (var inputDevice in inputDevices) {
                    if (inputDevice.Value) {
                        continue;
                    }

                    if (InputManager.GetButtonDown(inputDevice.Key, InputButton.Accept)) {
                        user = new User(inputDevice.Key, GetOpenColorIndex());

                        menuUser.SetUser(user);

                        colorsUse[user.colorIndex] = true;
                        gameManager.users[i] = user;
                        inputDevices[inputDevice.Key] = true;
                        break;
                    }
                }
            }
            else {
                if (InputManager.GetButtonDown(user.input, InputButton.Deny)) {
                    menuUser.RemoveUser();

                    colorsUse[user.colorIndex] = false;
                    gameManager.users[i] = null;
                    inputDevices[user.input] = false;
                    continue;
                }

                if (InputManager.GetButtonDown(user.input, InputButton.Start) && (menuUsers.Length > 1 ? GetUserCount() >= 2 : true)) {
                    menuUtil.StartGame();
                }
            }
        }
    }

    public int GetUserCount() {
        int count = 0;

        foreach (MenuUser menuUser in menuUsers) {
            if (menuUser.HasUser()) {
                count++;
            }
        }

        return count;
    }

    public int GetOpenColorIndex() {
        for (int colorIndex = 0; colorIndex < colorsUse.Length; colorIndex++) {
            if (!colorsUse[colorIndex]) {
                return colorIndex;
            }
        }

        return -1;
    }

    public int GetNextColor(int index, bool increase) {
        colorsUse[index] = false;
        //
        do {
            index = increase ? index + 1 : index - 1;

            if (index > colorsUse.Length - 1) {
                index = 0;
            }
            else if (index < 0) {
                index = colorsUse.Length - 1;
            }
        } while (colorsUse[index]);

        colorsUse[index] = true;
        return index;
    }
}
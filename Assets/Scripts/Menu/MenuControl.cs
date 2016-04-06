using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class MenuControl : MonoBehaviour {
    private MenuPanel[] menuPanels;

    void Awake() {
        menuPanels = GetComponentsInChildren<MenuPanel>(true);

        foreach (MenuPanel menuPanel in menuPanels) {
            menuPanel.gameObject.SetActive(false);
        }

        menuPanels[0].gameObject.SetActive(true);
    }

    public void ShowPanel(MenuPanel menuPanel) {
        foreach (MenuPanel _menuPanel in menuPanels) {
            if (_menuPanel) {
                _menuPanel.gameObject.SetActive(menuPanel.Equals(_menuPanel));
            }
        }
    }

    public void ShowPopupPanel(MenuPanel menuPanel){
        foreach (MenuPanel _menuPanel in menuPanels) {
            if (_menuPanel.Equals(menuPanel)) {
                menuPanel.gameObject.SetActive(true);
            }
        }
    }

    public void HidePanel(MenuPanel menuPanel) {
        menuPanel.gameObject.SetActive(false);
    }
}

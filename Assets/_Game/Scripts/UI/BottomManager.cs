using System;
using UnityEngine;

public class MainMenu_UiManager : MonoBehaviour
{
    public GameObject mainMenuContainer;
    public GameObject loadMenuContainer;
    public GameObject optionsMenuContainer;
    
    public GameObject currentMenu;

    private void Awake()
    {
        currentMenu = mainMenuContainer;
    }

    public void OpenLoadMenu()
    {
        currentMenu.SetActive(false);
        
        loadMenuContainer.SetActive(true);
        currentMenu = loadMenuContainer;
    }

    public void OpenMainMenu()
    {
        currentMenu.SetActive(false);
        
        mainMenuContainer.SetActive(true);
        currentMenu = mainMenuContainer;
    }
    
    public void OpenOptionsMenu()
    {
        currentMenu.SetActive(false);
        optionsMenuContainer.SetActive(true);
        currentMenu = optionsMenuContainer;
    }
}
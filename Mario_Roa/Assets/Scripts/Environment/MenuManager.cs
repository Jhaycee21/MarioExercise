using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1;
    }

    //3 buttons has a same function but we will get the button name to determine which level and scene we should load
    public void TravelToGameScene()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "Level1")
        {
            SceneManager.LoadScene("Game1");
        }

        if (EventSystem.current.currentSelectedGameObject.name == "Level2")
        {
            SceneManager.LoadScene("Game2");
        }

        if (EventSystem.current.currentSelectedGameObject.name == "Level3")
        {
            SceneManager.LoadScene("Game3");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

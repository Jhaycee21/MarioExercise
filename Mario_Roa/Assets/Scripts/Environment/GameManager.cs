using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Variable for score value
    public int score;

    //variable for displaying score value through text
    public Text scoreTxt;

    //variable for displaying final score value through text
    public Text scoreResultTxt;

    //variable for displaying highscore value through text
    public Text highScoreTxt;

    //Update the Score that need to display
    public GameObject ScoreResultPanel;

    //Panel fo game over
    public GameObject GameOverPanel;

    //panel for completing the level
    public GameObject CompletePanel;

    //panel for pause menu
    public GameObject PausePanel;

    //variable to store our player
    public GameObject Player;

    //variable to store our Background Music
    public GameObject BGM;

    //panel for our mobile control
    public GameObject ControlPanel;



    private void Start()
    {
        //Making sure that the game can be run at start
        Time.timeScale = 1;

        //Making sure that the control panel is on
        Invoke("HappenOnStartWithDelay", 1);
    }

    public void HappenOnStartWithDelay()
    {
        //Enable Only if build are for android
#if UNITY_ANDROID

        ControlPanel.SetActive(true);

#endif
    }

    //We will call this function if the player win the level
    public void Win()
    {
        ScoreResult();
        CompletePanel.SetActive(true);
        ScoreResultPanel.SetActive(true);
        BGM.SetActive(false);

        Destroy(Player.gameObject);
    }

    //We will call this function if the player lose the level
    public void Lose()
    {
        ScoreResult();
        GameOverPanel.SetActive(true);
        ScoreResultPanel.SetActive(true);

        Destroy(Player.gameObject);
    }

    //function that hold the displaying of scores
    void ScoreResult()
    {
        //saving current score using playerprefs
        PlayerPrefs.SetInt("currentScore", score);

        //check if playerprefs highscore exist
        if (PlayerPrefs.HasKey("highscore"))
        {
            //if player prefs high score exist, compare the current score and existing highscore
            if(PlayerPrefs.GetInt("currentScore") > PlayerPrefs.GetInt("highscore"))
            {
                PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("currentScore"));
            }
            else
            {
                Debug.Log("HighScore will Remain");
            }
        }
        //if playerprefs highscore doesnt exist create it and give it a value from the current score
        else
        {
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("currentScore"));
        }

        scoreResultTxt.text = scoreTxt.text;

    }


    //covert int to string to be able to display score and highscore through text
    private void Update()
    {
        scoreTxt.text = score.ToString();
        highScoreTxt.text = PlayerPrefs.GetInt("highscore").ToString();
    }

    //function of restart buttons, getting the current scene name to load exactly the same scene
    public void LoadCurrentGame()
    {
        if(SceneManager.GetActiveScene().name == "Game1")
        {
            SceneManager.LoadScene("Game1");
        }
        if (SceneManager.GetActiveScene().name == "Game2")
        {
            SceneManager.LoadScene("Game2");
        }
        if (SceneManager.GetActiveScene().name == "Game3")
        {
            SceneManager.LoadScene("Game3");
        }
    }


    //function of back to menu buttons
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    //function to pause the game by removing the value of time scale
    public void DoPause()
    {
        PausePanel.SetActive(true);

        Time.timeScale = 0;
    }


    public void DoResume()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }




}

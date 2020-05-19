/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

Responsible for scene transitions
*/
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    string thisScene; //name of the scene script is being run on
    string sceneToLoad; //name of scene you want to load
    string lastScene;
    int lastSceneWon;

    public Text lastScore;
    string scoreString;
    int scoreInt;

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
        if (lastScore != null)
        {
            SetScore();
        }
    }

    /* LoadLevel()
     * Generic level loader
     * used on the loadgame button to load wherever player left off
     */ 
    public void LoadLevel()
    {
        lastScene = PlayerPrefs.GetString("lastLoadedScene");
        SceneManager.LoadScene(lastScene, LoadSceneMode.Single);
    }

    /* LoadNextLevel()
     * used on next level screen after player wins
     * will load the next scene
     */ 
    public void LoadNextLevel()
    {
        lastSceneWon = PlayerPrefs.GetInt("lastSceneWon");
        SceneManager.LoadScene("Level" + (lastSceneWon + 1), LoadSceneMode.Single);
    }

    /* LoadLevel()
     * string argument used to load exact scene
     * if loading level 1 and boolean new game == true, reset the stats and load level 1
     */
    public void LoadLevel(string scene)
    {
        if (scene.Equals("Level1") && thisScene.Equals("TitleScreen"))
        {
            PlayerPrefs.DeleteAll();
        }
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    /* SetScore()
     * used for telling user what score they got in the last level
     * activated on nextlevel scene
     * if score is perfect, text is green
     * else, score is red
     */ 
    public void SetScore()
    {
        scoreString = null;
        scoreInt = PlayerPrefs.GetInt("level" + PlayerPrefs.GetInt("lastSceneWon") + "livesTemp");
        for (int i=0; i<scoreInt; i++)
        {
            scoreString += "* ";
        }
        if (scoreInt < 5)
        {
            lastScore.color = Color.red;
        }
        else
        {
            lastScore.color = new Color32(82, 167, 75, 255);
        }
        lastScore.text = scoreString;
    }

    public void Exit()
    {
        Application.Quit();
    }

}

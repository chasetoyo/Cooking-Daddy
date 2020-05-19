/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

Level Select screen mechanics
*/

using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private Button levelButton;
    private int livesInt;
    private string lives;

	void Start ()
    {
        SetButtonText();
        GameObject.Find("Level1").GetComponent<Button>().interactable = true; //level 1 should always be accessible
    }

    /* SetButtonText()
     * each button is labeled Level1, Level2, etc
     * gets the high score from each level and displays it on button text
     * if the level isn't unlocked, disables the button corresponding to level
     */ 
    void SetButtonText()
    {
        for (int i = 1; i < 13; i++)
        {
            levelButton = GameObject.Find("Level" + i).GetComponent<Button>();
            livesInt = PlayerPrefs.GetInt("level" + i + "lives");
            lives = null;

            for (int e = 0; e < livesInt; e++)
            {
                lives += "* ";
            }

            levelButton.GetComponentInChildren<Text>().text = "Level " + i + "\n" + lives;
            Debug.Log("status of level " + (i+1) + ": " + PlayerPrefs.GetInt("Level" + (i + 1)));
            if (PlayerPrefs.GetInt("Level" + (i)) == 1)
            {
                levelButton.interactable = true;
            }
            else
            {
                levelButton.interactable = false;
            }
        }
    }


}

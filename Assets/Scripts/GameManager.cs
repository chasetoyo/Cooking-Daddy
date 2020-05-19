/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

Responsible for managing the game's main mechanics
*/
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Dishes> dishesToMake; //list of dishes for each level
    public List<Ingredients> everyIngredient; //list of the ingredients that will drop
    public List<Ingredients> spiceList; //list of spices that are available
    public Sprite[] spriteList; // array of sprites used for the falling ingredients
    private GameObject[] fallingThings; //array of current food objects in scene 
    private GameObject[] ingredientUI; //array of ingredients for the current dish

    public SoundEffects soundFile; //create object from class that i created to reference for the soundeffects

    //interactable game objects
    public GameObject cloneBasket;
    public GameObject basket;
    public GameObject cloneFalling;
    public GameObject fallingIngredientGO;

    //basic UI
    private Text ingredientText;
    public Text dishToMakeGO;
    public Text scoreText;
    public Text livesText;
    public Text countdownGO;


    public string dishToMake; //top of screen
    private string fallingIngredient; //string component of falling ingredient
    private string livesString; //string component of stars. top left
    private string currentIngredient; //string component of ingredient that collided

    int sceneNumber;
    public int foodStreak;
    public int lives = 5; //start with 5 lives every round
    public float fallSpeed;

    public float xMin; //constraints for basket to fall between
    public float xMax;
    public float spawnTime; //how often ingredients fall
    public float countdown; //time before game starts

    /*game starts with canBoost false so boosts dont carry over*/
    public bool canSpeedBoost = false;
    public bool canSlowDown = false;
    public bool boostActive = false;

    /*private variables used in basketmanager/wallmanager*/
    bool containsIngredient;
    bool isSpice;

    /*instantiate a vector3 for ingredients to fall from, modified in code*/
    private Vector3 ingredientPos = new Vector3(0, 0, 0);

    Dishes currentDish = new Dishes();
    LoadScene sceneLoader = new LoadScene();

    /* Start()
     * Calls setup function and causes ingredients to fall after a certain delay
     */
    void Start()
    {
        Setup();
        InvokeRepeating("SetFallingIngredient", countdown, spawnTime);
    }

    /* Update()
     * Sets the countdown, when cd is close to 0, displays start message
     * checks if the user wants to use their bonus
     */ 
    void Update()
    {
        countdown -= Time.deltaTime;
        countdownGO.color = Color.red;
        countdownGO.text = countdown.ToString("f2");
        if (countdown < .5f)
        {
            countdownGO.color = Color.green;
            countdownGO.text = "START";
            if (countdown < 0f)
                countdownGO.enabled = false;
        }
        BonusPossible(foodStreak);
    }

    /* Setup()
     * Encapsulates all the various setters into one function
     * Saves current scene to player prefs for player to load into when they load game
     */ 
    void Setup()
    {
        SetDish();
        SetBasket();
        SetIngredientUI();
        SetScore();
        SetLives();
        PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
    }

    /* SetScore()
     * updates streak text 
     * called in basketmanager/wallmanager    
     */ 
    public void SetScore()
    {
        scoreText.text = "Streak: " + foodStreak;
    }

    /* SetLives()
     * looks for global variable lives
     * adds 1 star per 1 life
     * sets the lives text
     * called in basketmanager/wallmanager
     */ 
    void SetLives()
    {
        livesString = null;
        for (int i = 0; i < lives; i++)
        {
            livesString += "*";
        }
        livesText.text = livesString;
    }

    /* SetDish()
     * Because the first scene is title screen, level 1 would be at index 1
     * need index 0, corresponds to the index on list of dishes
     * Sets the UI for the dish as well as the dish GO which will be used for comparison
     */ 
    void SetDish()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex - 1;
        dishToMake = dishesToMake[sceneNumber].dishName; //set string
        currentDish = dishesToMake[sceneNumber]; //set actual game object
        dishToMakeGO.text = dishToMake;  //updates text to be the string
        currentDish.SetListCopy(currentDish.ingredientList); //sets copy of dish's ingredient list
    }

    /* SetBasket()
     * create basket GO
     */
    void SetBasket()
    {
        cloneBasket = Instantiate(basket, basket.transform.position, basket.transform.rotation) as GameObject;
    }

    /* SetIngredientUI()
     * looks at dishGO's ingredients
     * iterates through them and creates a text object on the left side of the screen
     */ 
    void SetIngredientUI()
    {
        float ingredientYPos = -100f;
        int n = 0;
        for (int i = 0; i < currentDish.ingredientList.Count; i++)
        {
            CreateText(currentDish.ingredientList[i], ingredientYPos, 25, Color.black, n);
            n++;
            ingredientYPos -= 40f;
        }
    }

    /* SetFallingIngredient()
     * The actual interactable ingredient GO
     * random ingredient falls randomly between the walls fro the same height
     * creates a clone that will eventually be destroyed without destroying the original
     * The velocity that it falls at is set in the editor, increases proportional to level
     */ 
    void SetFallingIngredient()
    {
        ingredientPos = new Vector3(Random.Range(xMin, xMax), 4, 0);
        int randomIngredientIndex = Random.Range(0, everyIngredient.Count);
        fallingIngredient = everyIngredient[randomIngredientIndex].ingredientName; //string name setter
        TextMesh ingredientText = fallingIngredientGO.GetComponentInChildren<TextMesh>();
        ingredientText.text = fallingIngredient; //text component of gameobject set to the string name
        cloneFalling = Instantiate(fallingIngredientGO, ingredientPos, Quaternion.identity) as GameObject;
        cloneFalling.GetComponent<Rigidbody>().velocity = new Vector3(0, fallSpeed, 0);
        cloneFalling.GetComponentInChildren<SpriteRenderer>().sprite = AssignSprite(fallingIngredient);
    }


    /*
     * controls the basket
     * takes a collider as argument, used to see what objects are caught
     * increase streak if collides with correct ingredient
     * increase stars if collides with spice
     * take away star if wrong ingredient
     * Updates the ingredient lists UI and sets boost booleans 
     */
    public void BasketManager(Collider other)
    {
        currentIngredient = other.GetComponentInChildren<TextMesh>().text.ToString();
        containsIngredient = false;
        isSpice = false;
        for (int i = 0; i < currentDish.ingredientListCopy.Count; i++) //iterates through the dishes list of ingredients
        {
            if (currentIngredient.Equals(currentDish.ingredientListCopy[i])) //checks to see if the string is the same as dishes ingredients
            {
                containsIngredient = true; //if so, then true. if not, then its defaulted to be false
            }
        }
        for (int i = 0; i < spiceList.Count; i++)
        {
            if (currentIngredient.Equals(spiceList[i].ingredientName))
            {
                containsIngredient = true;
                isSpice = true;
            }
        }

        if (containsIngredient == true) //if the thing was in the list, add a point.
        {
            soundFile.PlayCorrect();
            /*if its not a spice and i dont have a boost
             * increment the streak
             */ 
            if (isSpice == false)
            {
                if (boostActive == false)
                {
                    foodStreak++;
                    SetScore();
                }                
                ChangeToBlue(currentIngredient, 0);
            }
            else
            {
                if (lives < 5)
                {
                    lives++;
                    SetLives();
                }
            }
        }
        else if (containsIngredient == false)
        {
            lives--;
            foodStreak = 0;
            if (boostActive == false)
            {
                canSpeedBoost = false;
                canSlowDown = false;
            }
            scoreText.color = Color.black; 
            SetScore();
            SetLives();
            soundFile.PlayWrong();
        }
        CheckGameOver();
    }

    /* WallManager()
     * takes collider as argument, used to see what objects are caught
     * decrease life if i dropped an ingredient that was on the ingredient list
     * resets streak to 0 if dropped an ingredient that was on ingredient list
     */ 
    public void WallManager(Collider other)
    {
        currentIngredient = other.GetComponentInChildren<TextMesh>().text.ToString();
        containsIngredient = false;
        for (int i = 0; i < currentDish.ingredientListCopy.Count; i++) //iterates through the dishes list of ingredients
        {
            if (currentIngredient.Equals(currentDish.ingredientListCopy[i])) //checks to see if the string is the same as dishes ingredients
            {
                containsIngredient = true; //if so, then true. if not, then its defaulted to be false
            }
        }

        if (containsIngredient == true) //if the thing was in the list, fail.
        {
            lives--;
            SetLives();
            CheckGameOver();
            soundFile.PlayWrong();
            foodStreak = 0;
            scoreText.color = Color.black;
            SetScore();
            if (boostActive == false)
            {
                canSpeedBoost = false;
                canSlowDown = false;
            } 
        }
    }

    /* BonusPossible()
     * takes an int as argument, checks to see if it meets bonus requirements
     * sets boost booleans to be used in actual boost functions
     * visual feedback of color change in streak to tell player they can boost
     */ 
    public void BonusPossible(int streak)
    {
        if (foodStreak > 2) //catch 3 in a row then i can speed boost!
        {
            canSpeedBoost = true;
            scoreText.color = Color.blue;
        }
        if (foodStreak > 4) //catch 5 in a row then i can slow down the fall speed 
        {
            canSpeedBoost = false;
            canSlowDown = true;
            scoreText.color = Color.green;
            SlowDown();
        }
    }

    /* SlowDown()
     * if they press the boost button and they can boost
     * slows down time for falling objects, keep same for basket
     */ 
    public void SlowDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSlowDown)
        {
            soundFile.PlaySlow();
            boostActive = true;
            fallSpeed = fallSpeed / 2;
            spawnTime = spawnTime * 3;
            fallingThings = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject x in fallingThings)
            {
                x.GetComponent<Rigidbody>().velocity = new Vector3(0, fallSpeed, 0);
            }
            Invoke("ResetFallSpeed", 3f);
        }
    }

    /* ResetFallSpeed()
     * sets fall speed back to original
     * resets the streak
     */ 
    public void ResetFallSpeed()
    {
        boostActive = false;
        canSlowDown = false;
        fallSpeed = fallSpeed * 2;
        spawnTime = spawnTime / 3;
        fallingThings = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject x in fallingThings)
        {
            x.GetComponent<Rigidbody>().velocity = new Vector3(0, fallSpeed, 0);
        }
        scoreText.color = Color.black;
        foodStreak = 0;
        SetScore();
    }

    /* ChangeToBlue()
     * takes string, used for searching gameobject name
     * int used for indexing in Checking() function
     * changes Text color to blue
     * removes GO from the copied list
     */ 
    public void ChangeToBlue(string s, int n)
    {
        GameObject x = GameObject.Find(s);

        if (x.GetComponent<Text>().color == Color.black)
        {
            x.GetComponent<Text>().color = Color.blue;
        }
        else //if the color is already blue
        {
            Checking(x, s, n);
        }
        currentDish.ingredientListCopy.Remove(s);
    }

    /* Checking()
     * if there are multiples of the same ingredient, name is "Ingredient2" "Ingredient3" etc
     * searches for the game object, increments the number if cant find it
     * once it finds it, changes the GO color to blue
     */ 
    public GameObject Checking(GameObject x, string s, int n)
    {
        x = GameObject.Find(s + (n+1));
        int g = n;
        while (x == null) //if that doesn't exist, then keep adding numbers until it does exist
        {
            x = GameObject.Find(s + n);
            n++;
            g = n;
        }
        if (x.GetComponent<Text>().color == Color.black)
        {
            x.GetComponent<Text>().color = Color.blue;
        }
        else
        {
            Checking(x, s, g);
        }
        return x;
    }

    /* CreateText()
     * lots of technical things, in essence it will just create a text object using given arguments
     * obtained from stackoverflow
     */ 
    GameObject CreateText(string uiText, float y, int fontSize, Color textColor, int indexNO)
    {
        GameObject UItextGO;

        if (GameObject.Find(uiText) == null) //check if GO with name exists
        {
            UItextGO = new GameObject(uiText); //if it doesnt, create the GO
        }

        else
        {
            UItextGO = new GameObject(uiText+indexNO); //if it doeesn't create GO[n]
        }
      
        UItextGO.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector3(100f, y, 0);
        UItextGO.GetComponent<RectTransform>().sizeDelta = new Vector2(200,100);

        Text text = UItextGO.AddComponent<Text>();
        text.text = uiText;
        text.fontSize = fontSize;
        text.color = textColor;
        text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);//top left
        text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);

        UItextGO.tag = "Ingredient";

        return UItextGO;
    }

    /* CheckGameOver()
     * if theres nothing in the ingredient list, win
     * if lives = 0, lose
     * if win : sets high scores and allows player to access the next level 
     *          loads next level screen
     * if lose : loads the lose screen
     */ 
    public void CheckGameOver()
    {
        if (currentDish.ingredientListCopy.Count == 0) //win scenario
        {
            PlayerPrefs.SetInt("lastSceneWon", sceneNumber + 1);
            PlayerPrefs.SetInt("level" + (sceneNumber + 1) + "livesTemp", lives); //update the high score
           
            if (PlayerPrefs.GetInt("level" + (sceneNumber + 1) + "lives") < lives) //if the old score is less than current score
            {
                PlayerPrefs.SetInt("level" + (sceneNumber + 1) + "lives", lives); //update the high score
            }

            PlayerPrefs.SetInt("Level" + (sceneNumber + 2), 1);               //if i pass level1, then i'll save level(0+2) as acccessible

            if (sceneNumber == 11) //end of the game
            {
                sceneLoader.LoadLevel("GameOver");
            }

            sceneLoader.LoadLevel("NextLevel");
        }

        if (lives == 0) //lose scenario
        {
            sceneLoader.LoadLevel("LoseScreen");
        }
    }

    /* AssignSprite()
     * takes string as input to search for sprite name
     * returns the sprite with the provided name
     */ 
    public Sprite AssignSprite(string s)
    {
        Sprite spriteToUse = null;
        foreach (Sprite x in spriteList)
        {
            if (x.name.Equals(s))
                {
                    spriteToUse = x;
                }
        }
        return spriteToUse;
    }
}

/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

Responsible for basket movement and boosts
*/
using UnityEngine;

public class Basket : MonoBehaviour
{
    //limits speed and location of paddle
    public float baseSpeed;
    float paddleSpeed;
    public float limitLeft;
    public float limitRight;
    public float paddleYpos;
    private GameManager gm;
    private Vector3 playerPos = new Vector3(0, 0, 0);
    public float paddleBoost = 2f;

    /* Start()
     * sets temp variable to the original speed
     */ 
    void Start()
    {
        paddleSpeed = baseSpeed;
        Debug.Log("before: " + paddleSpeed);
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        PlayerMovement();
    }

    /* PlayerMovement()
     * if they press boost key and they can boost, boost basket speed
     */ 
    void PlayerMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gm.canSpeedBoost)
        {
            gm.soundFile.PlayFast();
            Debug.Log("BOOST ACTIVATED");
            gm.boostActive = true;
            paddleSpeed = baseSpeed * paddleBoost;
            Invoke("ResetBoost", 3f);
        }

        if (!gm.canSpeedBoost)
        {
            paddleSpeed = baseSpeed;
        }

        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
        playerPos = new Vector3(Mathf.Clamp(xPos, limitLeft, limitRight), paddleYpos, 0f);
        transform.position = playerPos;
    }

    /* ResetBoost()
     * resets boost booleans and the streak(UI)
     */ 
    void ResetBoost()
    {
        gm.canSpeedBoost = false;
        gm.boostActive = false;
        gm.scoreText.color = Color.black;
        gm.foodStreak = 0;
        gm.SetScore();
        Debug.Log("Boost was reset");
    }
}

/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

Responsible for destroying game objects if they touch the wall
*/
using UnityEngine;

public class WallDestroyIngredient : MonoBehaviour
{
    private GameManager gm;
    
    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /* OnTriggerEnter()
     * takes collider, used to check what object collided with
     * calls functionality from gamemanager
     * destroys object if its a food
     */ 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            gm.WallManager(other);
            Destroy(other.gameObject);
        }   
    }

}

/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

Responsible for destroying game objects that collide with basket
*/
using UnityEngine;

public class BasketDestroyIngredient : MonoBehaviour
{
    private GameManager gm;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /* OnTriggerEnter()
     * takes collider, used to see what object collided with
     * calls basket functionality from game manager
     * destroys collided object if its a food
     */ 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            gm.BasketManager(other);
            Destroy(other.gameObject);
        }
    }
}

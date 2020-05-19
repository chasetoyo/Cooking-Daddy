/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

used to create Dishes GO
*/
using System.Collections.Generic;
using UnityEngine;

public class Dishes : MonoBehaviour
{
    public List<string> ingredientList;
    public List<string> ingredientListCopy;
    public string dishName;

    /* SetListCopy()
     * takes a list of strings as argument
     * makes a deepcopy of list
     */ 
    public void SetListCopy(List<string> list)
    {
        ingredientListCopy.Clear(); //reset the list after every play
        foreach (var i in list)
        {
            ingredientListCopy.Add((string)i.Clone()); //make a deep copy of our ingredient list
        }
    }
} 


/*
Name: Chase Toyofuku-Souza
Student ID#: 2296478
Chapman email: toyofukusouza @chapman.edu
Course Number and Section: CPSC 236-02
Assignment: 05 - Cooking Daddy

sound effects for various event triggers
*/
using UnityEngine;

public class SoundEffects : MonoBehaviour
{

    public AudioClip correctIngredient;
    public AudioClip wrongIngredient;
    public AudioClip slowDown;
    public AudioClip speedUp;

    private AudioSource source;

    /* Start()
     * assigns source to the game objects audio source component
     */ 
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    /* These all do the same thing but with a different audioclip
     */ 
    public void PlayCorrect()
    {
        source.PlayOneShot(correctIngredient, 1f);
    }

    public void PlayWrong()
    {
        source.PlayOneShot(wrongIngredient, .5f);
    }

    public void PlaySlow()
    {
        source.PlayOneShot(slowDown, 1f);
    }

    public void PlayFast()
    {
        source.PlayOneShot(speedUp, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [Header("Things to disable when player dies")]
    [SerializeField] GameObject healthUI;
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject gunSightUI;
    [SerializeField] PlayerScript playerScript;

    //This function is triggered when this object is enabled
    private void OnEnable()
    {
        //Finding the player script in the heirachy
        playerScript = FindObjectOfType<PlayerScript>();
        //subscribing the death screen function to the player script death event
        playerScript.deathEvent += deathScreen;
    }

    public void deathScreen()
    {
        //Disabling the Health UI
        healthUI.SetActive(false);

        //Disabling the score UI
        scoreUI.SetActive(false);

        //Disabling the gun sight UI
        gunSightUI.SetActive(false);

        //unlocking the mouse from the screen
        Cursor.lockState = CursorLockMode.None;

        //making the mouse visible to the screen
        Cursor.visible = true;

        //Pausing the game
        Time.timeScale = 0;
    }
}

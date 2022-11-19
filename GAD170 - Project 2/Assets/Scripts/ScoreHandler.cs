using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Text scoreValue;
    [SerializeField] TextMeshProUGUI endScore;
    [SerializeField] Text healthValue;
    [SerializeField] PlayerScript playerScript;

    public int totalScore;

    void Update()
    {
        //Responsible for the health UI
        HealthValueUI();

        //Responsible for the Score UI
        ScoretextUI();
    }

    void HealthValueUI()
    {
        //If the player script is detected then set the UI text value with the player health
        if (playerScript != null)
        {
            healthValue.text = playerScript.PlayerHealth().ToString();
        }
    }

    void ScoretextUI()
    {
        //Setting the score text in the UIs with the score gathered from the player
        scoreValue.text = totalScore.ToString("000000");
        endScore.text = totalScore.ToString("000000");
    }

    //get and set Function

    //Used for adding the score to the total score
    public void AddScore(int score)
    {
        totalScore += score;
    }

}// End of Class

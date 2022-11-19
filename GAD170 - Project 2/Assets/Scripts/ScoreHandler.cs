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

    public int TotalScore;

    // Update is called once per frame
    void Update()
    {
        HealthValueUI();
        ScoretextUI();
    }

    void HealthValueUI()
    {
        if (playerScript != null)
        {
            healthValue.text = playerScript.PlayerHealth().ToString();
        }
    }

    void ScoretextUI()
    {
        scoreValue.text = TotalScore.ToString("000000");
        endScore.text = TotalScore.ToString("000000");
    }

    //get and set Function
    public void AddScore(int score)
    {
        TotalScore += score;
    }



}// End of Class

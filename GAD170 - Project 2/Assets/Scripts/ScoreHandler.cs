using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Text ScoreValue;
    [SerializeField] TextMeshProUGUI EndScore;
    [SerializeField] Text HealthValue;
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
            HealthValue.text = playerScript.PlayerHealth().ToString();
        }
    }

    void ScoretextUI()
    {
        ScoreValue.text = TotalScore.ToString("000000");
        EndScore.text = TotalScore.ToString("000000");
    }

    //get and set Function
    public void AddScore(int score)
    {
        TotalScore += score;
    }



}// End of Class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarRotation : MonoBehaviour
{
    GameObject player;
    Slider slider;
    
    private void Start()
    {
        //Finding the game object with a player tag
        player = GameObject.FindGameObjectWithTag("Player");
        //Getting the slider component of this object
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        //This UI should always look at the player
        transform.LookAt(player.transform);
    }

    //Used for setting the health bar value of this UI
    public void SetHealthBar(int health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }
}

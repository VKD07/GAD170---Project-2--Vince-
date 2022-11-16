using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarRotation : MonoBehaviour
{
    GameObject Player;
    Slider slider;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform);
    }

    public void SetHealthBar(int Health)
    {
        if (slider != null)
        {
            slider.value = Health;
        }
    }
}

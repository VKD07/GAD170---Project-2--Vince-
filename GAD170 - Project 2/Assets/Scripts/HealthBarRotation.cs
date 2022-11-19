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
        player = GameObject.FindGameObjectWithTag("Player");
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }

    public void SetHealthBar(int health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }
}

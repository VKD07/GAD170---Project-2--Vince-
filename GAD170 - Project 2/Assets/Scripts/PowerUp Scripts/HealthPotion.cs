using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int hpValue = 20;
    PlayerScript playerScript;

    void Start()
    {
        //Finding the game object with the Player script component
        playerScript = FindObjectOfType<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the health power up collided with the player then add health to the player
        //then destroy this object
        if (other.tag == "Player")
        {
            playerScript.AddPlayerHealth(hpValue);
            Destroy(gameObject);
        }
    }
}

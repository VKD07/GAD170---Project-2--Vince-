using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int hpValue = 20;
    PlayerScript playerScript;


    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript.AddPlayerHealth(hpValue);
            Destroy(gameObject);
        }
    }
}

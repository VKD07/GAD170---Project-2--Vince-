using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    [SerializeField] GameObject healthUI;
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject gunSightUI;
    [SerializeField] PlayerScript playerScript;

    private void OnEnable()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        playerScript.deathEvent += deathScreen;
    }

    public void deathScreen()
    {
        healthUI.SetActive(false);
        scoreUI.SetActive(false);
        gunSightUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}

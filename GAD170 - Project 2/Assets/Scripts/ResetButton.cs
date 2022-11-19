using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    //This function is called whenever the play again button is clicked.
    //This will reset the scene
    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
}

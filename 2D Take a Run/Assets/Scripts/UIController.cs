using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public void loadGamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
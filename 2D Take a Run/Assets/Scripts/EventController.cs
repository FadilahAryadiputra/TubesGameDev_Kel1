using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    public void loadGamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
}

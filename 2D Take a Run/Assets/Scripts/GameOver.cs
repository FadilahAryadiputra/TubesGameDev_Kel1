using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    PlayerController player;

    public Text finalDistanceText;
    public Text finalScoreText;
    public Text highScoreText;
    int highscorePoint;
    int highscoreDistance;

    // Start is called before the first frame update
    void Start()
    {
        highscorePoint = PlayerPrefs.GetInt("HSpoint", 0);
        highscoreDistance = PlayerPrefs.GetInt("HSdistance", 0);

        finalDistanceText.text = ShowScore.finalDistance + " m";
        finalScoreText.text = ShowScore.score.ToString("") + " p";

        if (ShowScore.score > highscorePoint)
        {
            highscorePoint = ShowScore.score;
            PlayerPrefs.SetInt("HSpoint", highscorePoint);
        }
        if (ShowScore.finalDistance > highscoreDistance)
        {
            highscoreDistance = ShowScore.finalDistance;
            PlayerPrefs.SetInt("HSdistance", highscoreDistance);
        }
        highScoreText.text = highscoreDistance.ToString("") + " m / " + highscorePoint.ToString("") + " p";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        SceneManager.LoadScene("GamePlay");
        ShowScore.score = 0;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

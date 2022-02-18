using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    PlayerController player;
    public Text distanceText;
    public Text scoreText;

    GameObject results;
    public Text finalDistanceText;
    public Text finalScoreText;
    public Text highScoreText;

    public static int score;
    int highscorePoint;
    int highscoreDistance;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        results = GameObject.Find("Results");

        results.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        highscorePoint = PlayerPrefs.GetInt("HSpoint", 0);
        highscoreDistance = PlayerPrefs.GetInt("HSdistance", 0);
    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";
        scoreText.text = score.ToString("0") + " p";

        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance + " m";
            finalScoreText.text = score.ToString("") + " p";
            if (score > highscorePoint)
            {
                highscorePoint = score;
                PlayerPrefs.SetInt("HSpoint", highscorePoint);
            }
            if (distance > highscoreDistance)
            {
                highscoreDistance = distance;
                PlayerPrefs.SetInt("HSdistance", highscoreDistance);
            }
        }
        highScoreText.text = highscoreDistance.ToString("") + " m / " + highscorePoint.ToString("") + " p";
    }

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
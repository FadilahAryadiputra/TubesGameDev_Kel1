using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    PlayerController player;
    public Text distanceText;
    public Text scoreText;

    public GameObject results;

    public static int score;
    public static int finalDistance;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        results.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        finalDistance = distance;
        distanceText.text = distance + " m";
        scoreText.text = score.ToString("0") + " p";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float depth = 1;

    PlayerController player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float realVelocity = 10 / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x <= -20)
            pos.x = 20;

        transform.position = pos;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerController player;
    Animator anim;
    private Rigidbody2D rb;
    public AudioSource isHitSound;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        if (pos.x < -20)
        {
            Destroy(gameObject);
        }

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Player"))
        {
            // rb.velocity = Vector2.zero;
            rb.AddForce(Physics.gravity * 8 * rb.mass);
            rb.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
            anim.SetBool("Die", true);
            isHitSound.Play();
            Destroy(this.gameObject, 2);
        }
    }
}

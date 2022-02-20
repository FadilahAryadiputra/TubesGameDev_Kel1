using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerController player;

    Animator anim;
    public GameObject Explosion;
    public AudioSource isDestroyedSound;
    public AudioSource isHitSound;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        if (col.transform.tag.Equals("HitArea"))
        {
            anim.SetBool("Die", true);
            Destroy(this.gameObject, 2);
            ShowScore.score += 20;
            isHitSound.Play();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag.Equals("Player"))
        {
            Instantiate (Explosion,transform.position,transform.rotation);
            Destroy(this.gameObject);
            isDestroyedSound.Play();
        }
    }
}

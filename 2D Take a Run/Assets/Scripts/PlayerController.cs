using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rb;

    public bool isDead = false;

    public bool isGrounded;//is my player colliding with the floor?
    public bool isJumping;//is my player jumping or can he jump?

    public float gravity;
    public Vector2 velocity;
    public float groundHeight = 10;
    public float distance = 0;
    public float maxXVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float jumpVelocity = 20;
    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;
    public float jumpGroundThreshold = 1;

    float timer = 0;
    public int numOfHearts = 3;
    public float healthAmount = 3;
    public Image[] hearts;
    public Sprite fullHearth;
    public Sprite emptyHeart;
    bool isCanAttack = true;
    public float attackCooldown = 0.5f;
    public GameObject hitArea;
    public Vector2 hitAreaOffset;

    public AudioSource jumpSound;
    public AudioSource attackSound;
    public AudioSource hurtSound;
    public AudioSource hitObstacleSound;
    public AudioSource deathSound;

    public GameObject gameOver;

    private void Awake()
    {
        gameOver.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isJumping=false;
        isCanAttack = true;
        healthAmount = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            isHoldingJump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }

        if(Input.GetKeyDown(KeyCode.Z))
		{
			Attack();
		}

        if (healthAmount <= 0)
        {
            isDead = true;
            gameOver.SetActive(true);
            deathSound.Play();
            Destroy(this.gameObject, 2);
            timer += Time.deltaTime;
            if(timer > 2){
                SceneManager.LoadScene("GameOver");
            }
        }

        if(healthAmount > numOfHearts){
            healthAmount = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++){

            if(i < healthAmount){
                hearts[i].sprite = fullHearth;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

    void FixedUpdate()
	{
        Vector2 pos = transform.position;

        distance += velocity.x * Time.fixedDeltaTime;

        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.deltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }
            pos.y += velocity.y * Time.deltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.deltaTime;
            }

            if (pos.y <= groundHeight)
            {
                pos.y = groundHeight;
                isGrounded = true;
                holdJumpTimer = 0;
                velocity.y = 0;
            }
        }

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }
        }


        transform.position = pos;
	}
    void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log ("you hit something: " + col.collider.tag);//basic print message for debugging purposes
		if (col.collider.tag == "Ground")//if the object you collided withs tag is ground your player is on the floor
		{
            anim.SetTrigger("Run");
            anim.ResetTrigger("Hurt");
            anim.ResetTrigger("Attack");
			isGrounded = true;///so grounded must be true because Player has hit the floor.
            isJumping = false;
            holdJumpTimer = 0;
            velocity.y = 0;
		}

        if (col.transform.tag.Equals("Enemy"))
        {
            hurtSound.Play();
            healthAmount -= 1f;
            velocity.x *= 0.7f;
            anim.SetBool("Hurt", true);
            timer += Time.deltaTime;
            if(timer > 0.2){
                anim.SetBool("Hurt", false);
                anim.SetTrigger("Run");
            }
        }
	}
    void OnCollisionExit2D(Collision2D col)
	{
		Debug.Log ("you left something: " + col.collider.tag);//basic print message for debugging purposes
		if (col.collider.tag == "Ground")//if the object you WERE colliding withs tag is ground your player is leaving the floor
		{
			isGrounded = false;//Player is no longer grounded
            anim.SetTrigger("Jump");
		}

	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Obstacle"))
        {
            velocity.x *= 0.7f;
            hitObstacleSound.Play();
            anim.SetTrigger("Hurt");
        }
        if (col.transform.tag.Equals("DeadZone"))
        {
            healthAmount = 0;
        }
    }

    public void JumpBtnKeyDown()
    {
        Jump();
        isHoldingJump = true;
    }
    public void JumpBtnKeyUp()
    {
        isHoldingJump = false;
    }
    public void AttackBtn()
    {
        Attack();
    }
    void Jump()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            {
                isGrounded = false;
                isJumping = true;
                jumpSound.Play();
                velocity.y = jumpVelocity;
                holdJumpTimer = 0;
            }
        }
    }

    void Attack()
    {
        if (isCanAttack)
        {
            StartCoroutine(CanAttack());
            // anim.SetTrigger("playerAttack");
            // GameObject hitPoint = Instantiate(hitArea.gameObject);
            anim.SetTrigger("Attack");
            attackSound.Play();
            GameObject hitPoint = Instantiate(hitArea, (Vector2)transform.position - hitAreaOffset * transform.localScale.x, Quaternion.identity);
            hitPoint.transform.parent = gameObject.transform;
        }
    }
    IEnumerator CanAttack()
    {
        // anim.SetTrigger("playerAttack");
        isCanAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        // anim.ResetTrigger("playerAttack");
        isCanAttack = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rb;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

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

    public int numOfHearts = 3;
    public float healthAmount = 3;
    public Image[] hearts;
    public Sprite fullHearth;
    public Sprite emptyHeart;
    bool isCanAttack = true;
    public float attackCooldown = 0.5f;
    public GameObject hitArea;
    public Vector2 hitAreaOffset;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        isJumping=false;
        isCanAttack = true;
        healthAmount = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                isJumping = true;
                isHoldingJump = true;
                velocity.y = jumpVelocity;
                holdJumpTimer = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.Z))
		{
			playerAttack();
		}

        if (healthAmount <= 0)
        {
            Destroy(this.gameObject);
            isDead = true;
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

        if(direction == 0 ){
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                direction = 1;
            } else if(Input.GetKeyDown(KeyCode.RightArrow)){
                direction = 2;
            } else if(Input.GetKeyDown(KeyCode.UpArrow)){
                direction = 3;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)){
                direction = 4;
            }
        } else {
            if(dashTime <= 0){
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            } else {
                dashTime -= Time.deltaTime;

                if(direction == 1){
                    rb.velocity = Vector2.left * dashSpeed;
                } else if(direction == 2){
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else if(direction == 3){
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if(direction == 4){
                    rb.velocity = Vector2.down * dashSpeed;
                }
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
			isGrounded = true;///so grounded must be true because Player has hit the floor.
            isJumping = false;
            holdJumpTimer = 0;
            velocity.y = 0;
		}

        if (col.transform.tag.Equals("Enemy"))
        {
            healthAmount -= 1f;
        }
	}
    void OnCollisionExit2D(Collision2D col)
	{
		Debug.Log ("you left something: " + col.collider.tag);//basic print message for debugging purposes
		if (col.collider.tag == "Ground")//if the object you WERE colliding withs tag is ground your player is leaving the floor
		{
			isGrounded = false;//Player is no longer grounded
		}

	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Obstacle"))
        {
            velocity.x *= 0.7f;
        }
        if (col.transform.tag.Equals("DeadZone"))
        {
            healthAmount = 0;
        }
    }

    void playerAttack()
    {
        if (isCanAttack)
        {
            StartCoroutine(CanAttack());
            // anim.SetTrigger("playerAttack");
            // GameObject hitPoint = Instantiate(hitArea.gameObject);
            GameObject hitPoint = Instantiate(hitArea, (Vector2)transform.position - hitAreaOffset * transform.localScale.x, Quaternion.identity);
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
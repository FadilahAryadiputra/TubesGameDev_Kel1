using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
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
    public float holdJumpTimer = 0.0f;
    public float jumpGroundThreshold = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        isJumping=false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        // if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))//Input.GetKeyDown() waits for the user to press a key once
		// {
		// 	if(isGrounded == true)//if my player is grounded do whats in the barckets
		// 	{
		// 		isJumping=true;
        //         velocity.y = jumpVelocity;
		// 	}
		// }

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
        if(Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene("SampleScene");
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
            }
        }

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);

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
}
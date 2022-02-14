using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float jumpForceValue = 200;
    public float jumpForce;//how much force is the player using to jump. gravity is always pushing down on player so he needs X amount of force to jump
    public float sidSpd;//how fast is my player moving left or right (Left this will be negative, RIght this willbe positive)
	public float moveSpd;//A variable were going to ue later on in the tutorial to increase players speed or decrease it

    public Vector2 velocity;
    public float distance = 0;
    public float maxXVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;

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

        sidSpd = Input.GetAxis ("Horizontal") * 5 ;//Input.GetAxis will grab the Axis "Horizontal" in this case.
		// to create axis  hit the Edit tab > Project Setting > Input

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))//Input.GetKeyDown() waits for the user to press a key once
		{
			if(isGrounded == true)//if my player is grounded do whats in the barckets
			{
				isJumping=true;
				jumpForce = jumpForceValue;//jump force equals 200 because our Player has attempted to jump
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

		rb.AddForce (new Vector3(sidSpd,jumpForce,0));//Add a force to my players RigidBody.
		//The force must be a vector 3 for this script so it adds force in 3 Dimensions.

        distance += velocity.x * Time.fixedDeltaTime;

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
		}
	}
    void OnCollisionExit2D(Collision2D col)
	{
		Debug.Log ("you left something: " + col.collider.tag);//basic print message for debugging purposes
		if (col.collider.tag == "Ground")//if the object you WERE colliding withs tag is ground your player is leaving the floor
		{
			isGrounded = false;//Player is no longer grounded
			jumpForce = 0;//the Player no longer has any force in his jump when he is in the air
		}

	}
}
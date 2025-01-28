using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalker : MonoBehaviour
{
    // Max speed player can move at
    public float speedMax = 10f;

    private float speedHorizCurrent;
    private float speedVertCurrent;

    // Player gravity. Shouldn't use this over unity's gravity scale.
    //public float gravity = 2f;

    // How high the player is to jump
    public float jumpHeight = 4f;
    // How long to buffer the jump input for. Currently nonfunctional.
    public float jumpBufferLength = .2f; // Currently unused
    private float jumpBufferCurrent; 

    // Is player touching the ground?
    private bool isGrounded;

    private Rigidbody2D myRB;

    // Spawnpoint of the bubble
    public Transform bubbleSpawn;
    // The Bubble
    public GameObject prefabBubble;
    public float fireCooldown;

    // Input stuff
    private float inputHoriz;
    private bool inputJump;
    private bool jumpHeld; // Currently unused, will be used to restrict jumping until jump input has been released.
    private Vector2 moveDirection;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        jumpBufferCurrent = 0f;
    }

    
    void Update()
    {
        if (jumpBufferCurrent > 0f)
        {
            jumpBufferCurrent -= 1f * Time.deltaTime;
            if (jumpBufferCurrent < 0f) 
            { 
                jumpBufferCurrent = 0f; 
            }
        }
        // Get the movement inputs
        GetInput();

        if (fireCooldown <= 0)
                {
                    fireCooldown = 0;
                    if (Input.GetAxisRaw("Fire1") > 0.01f)
                    {
                        FireBubble(bubbleSpawn.transform);
                    }
                }
    }

    void FixedUpdate()
    {
        // Can't directly modify the X/Y velocity, must copy their values into floats and modify those
        speedHorizCurrent = myRB.velocity.x;
        speedVertCurrent = myRB.velocity.y;

        // If they are grounded and jump is pressed, jump.
        if ((isGrounded == true) && (inputJump == true))
        {
            speedVertCurrent += jumpHeight;
            isGrounded = false;
            print("Jumped!");
        }

        //Debug.Log(isGrounded + "\n" + jumpBufferCurrent + "\n" + jumpHeld);
        
        // Only apply horizontal movement if speed isn't already at or above max speed
        if (speedHorizCurrent !> speedMax || true)
        {
            speedHorizCurrent = inputHoriz * speedMax;
        }

        // Update rigidbody velocity with new values
        moveDirection = new Vector2(speedHorizCurrent, speedVertCurrent);
        myRB.velocity = moveDirection;
    }

    // Get the inputs needed. Don't do any physics calculations here!
    void GetInput()
    {
        inputHoriz = Input.GetAxisRaw("Horizontal");
        if (Input.GetAxisRaw("Jump") > 0f)
        {
            inputJump = true;
        }

        else
        {
            inputJump = false;
            jumpHeld = false;
        }
        
    }
    
    void FireBubble(Transform p_bubbleSpawn)
    {

    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        // Check if collision is with ground and set isGrounded appropriately
        if (other.collider.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Not Grounded!");
        }
    }
}
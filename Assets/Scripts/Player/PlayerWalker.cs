using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerWalker : MonoBehaviour
{
    [Tooltip("Max speed player can move at")]
    public float speedMax = 10f;

    private float speedHorizCurrent;
    private float speedVertCurrent;
    private bool facingDirection = false; // false = left, true = right

    // Player gravity. Shouldn't use this over unity's gravity scale.
    //public float gravity = 2f;

    [Tooltip("How high the player is to jump")]
    public float jumpHeight = 4f;
    [Tooltip("How long to buffer the jump input for. Currently nonfunctional.")]
    public float jumpBufferLength = .2f; // Currently unused
    private float jumpBufferCurrent; 

    // Is player touching the ground?
    private bool isGrounded;

    private Rigidbody2D myRB;

    [Tooltip("Spawnpoint of the bubble")]
    public Transform bubbleSpawn;
    public Transform bubbleSpawnInverted; // 
    [Tooltip("The Bubble")]
    public GameObject prefabBubble;
    [Tooltip("Max num of bubbles that can be active at once. Default (3)")]
    public int maxBubbles = 3;
    [Tooltip("How fast can you shoot bubble? Default (.1)")]
    public float fireRate = .1f;
    private float fireCooldown = 0f; // Internal countdown

    // Input stuff
    private float inputHoriz;
    private bool inputJump;
    private bool inputJumpHeld; // Currently unused, will be used to restrict jumping until jump input has been released.
    private bool inputFire;
    private bool inputFireHeld;
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
            if (inputFire == true && inputFireHeld != true)
            {
                if (facingDirection) {
                    FireBubble(bubbleSpawn.transform);
                }
                else {
                    FireBubble(bubbleSpawnInverted.transform);
                }
                inputFireHeld = true;
            }
        }

        else
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Can't directly modify the X/Y velocity, must copy their values into floats and modify those
        speedHorizCurrent = myRB.velocity.x;
        speedVertCurrent = myRB.velocity.y;

        // If they are grounded and jump is pressed, jump.
        if ((isGrounded == true) && (inputJump == true) && (inputJumpHeld == false))
        {
            speedVertCurrent += jumpHeight;
            isGrounded = false;
            print("Jumped!");
            inputJumpHeld = true;
        }

        //Debug.Log(isGrounded + "" + inputJumpHeld);

        //Debug.Log(isGrounded + "\n" + jumpBufferCurrent + "\n" + inputJumpHeld);
        
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
        if (inputHoriz > 0.01f)
        {
            facingDirection = true;
        }
        else if (inputHoriz < -0.01f) 
        {
            facingDirection = false;
        }

        if (Input.GetAxisRaw("Jump") >= 0.01f)
        {
            inputJump = true;
        }

        else {
            inputJump = false;
            inputJumpHeld = false;
        }
        
        if (Input.GetAxisRaw("Fire1") > 0.01f) 
        {
            inputFire = true;
        }

        else {
            inputFire = false;
            inputFireHeld = false;
        }
    }
    

    void FireBubble(Transform p_bubbleSpawn)
    {
        if (GameObject.FindGameObjectsWithTag("BubbleShot").Length < maxBubbles)
        {
            fireCooldown = fireRate;
            Instantiate(prefabBubble, p_bubbleSpawn.position, p_bubbleSpawn.rotation);
        }
    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        // Check if collision is with ground and set isGrounded appropriately
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        //else
        //{
        //    isGrounded = false;
        //}
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
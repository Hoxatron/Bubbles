using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWalker : MonoBehaviour
{
    [Tooltip("Max speed player can move at")]
    public float speedMax = 10f;

    private float speedHorizCurrent;
    private float speedVertCurrent;
    private bool facingDirection = false; // false = left, true = right

    // Player gravity. Shouldn't use this over unity's gravity scale.
    //public float gravity = 2f;

    //Audio clips stored in variables
    public AudioSource jumpAudio;
    public AudioSource jumpBubbleAudio;
    public AudioSource shootAudio;

    [Tooltip("How high the player is to jump")]
    public float jumpHeight = 4f;
    [Tooltip("How long to buffer the jump input for. Currently nonfunctional.")]
    // private float jumpBufferLength = .2f; // Currently unused
    private float jumpBufferCurrent;

    // Is player touching the ground?
    [HideInInspector] public bool isGrounded;

    [HideInInspector] public bool isEmptyBouncing; // Is bouncing gonna happen
    [HideInInspector] public bool isFullBouncing; // Is bigger bouncing gonna happen
    [Tooltip("How high does player bounce off of empty bubbles")]
    public float emptyBounceHeight;
    [Tooltip("How high does player bounce off of full bubbles")]
    public float fullBounceHeight;

    private Rigidbody2D myRB;
    //private Collider2D lowerBounce;

    [Tooltip("Spawnpoint of the bubble when facing right")]
    public Transform bubbleSpawn;
    [Tooltip("The other spawnpoint of the bubble, when facing left")]
    public Transform bubbleSpawnInverted; // 
    [Tooltip("The Bubble")]
    public GameObject prefabBubble;
    [Tooltip("Max num of bubbles that can be active at once.")]
    public int maxBubbles = 3;
    [Tooltip("How fast can you shoot bubble?")]
    public float fireRate = .1f;
    private float fireCooldown = 0f; // Internal countdown for bubble fire rate.

    // Input stuff
    private float inputHoriz;
    private bool inputJump;
    private bool inputJumpHeld; // Used to restrict jumping until button is released.
    private bool inputFire;
    private bool inputFireHeld; // Used to restrict firing until button is released.
    private Vector2 moveDirection;

    public Animator bobbyAnim;
    public GameObject Bobby;

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
                if (facingDirection)
                {
                    FireBubble(bubbleSpawn.transform);
                }
                else
                {
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
            jumpAudio.Play();
            isGrounded = false;
            bobbyAnim.SetTrigger("JumpTrigger");

            inputJumpHeld = true;
        }

        // Only apply horizontal movement if speed isn't already at or above max speed
        if (speedHorizCurrent! > speedMax || true)
        {
            speedHorizCurrent = inputHoriz * speedMax;
        }

        // If bouncing on bubble, 
        if (isEmptyBouncing == true)
        {
            speedVertCurrent = emptyBounceHeight;
            jumpBubbleAudio.Play();
        }
        else if (isFullBouncing == true)
        {
            speedVertCurrent = fullBounceHeight;
            jumpBubbleAudio.Play();
        }

        

            // Update rigidbody velocity with new values
            moveDirection = new Vector2(speedHorizCurrent, speedVertCurrent);
        myRB.velocity = moveDirection;

        isEmptyBouncing = false;
        isFullBouncing = false;
    }

    // Get the inputs needed. Don't do any physics calculations here!
    void GetInput()
    {
        inputHoriz = Input.GetAxisRaw("Horizontal");
        if (inputHoriz > 0.01f)
        {
            facingDirection = true;
            Bobby.transform.rotation = new Quaternion(0, 180, 0, 0);
            if (isGrounded == true)
            {
                bobbyAnim.SetTrigger("WalkRight");
            }
        }
        else if (inputHoriz < -0.01f)
        {
            facingDirection = false;
            Bobby.transform.rotation = new Quaternion(0, 0, 0, 0);
            if (isGrounded == true)
            {
                bobbyAnim.SetTrigger("WalkLeft");
            }
        }

        if (Input.GetAxisRaw("Jump") >= 0.01f)
        {
            inputJump = true;
        }

        else
        {
            inputJump = false;
            inputJumpHeld = false;
        }

        if (Input.GetAxisRaw("Fire1") > 0.01f)
        {
            inputFire = true;
        }

        else
        {
            inputFire = false;
            inputFireHeld = false;
        }

        if (inputHoriz == 0f)
        {
            if (isGrounded == true)
            {
                bobbyAnim.SetTrigger("Idle");
            }
        }

    }


    void FireBubble(Transform p_bubbleSpawn)
    {
        if (GameObject.FindGameObjectsWithTag("BubbleShot").Length < maxBubbles)
        {
            fireCooldown = fireRate;
            Instantiate(prefabBubble, p_bubbleSpawn.position, p_bubbleSpawn.rotation);
            shootAudio.Play();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // Check if collision is with ground and set isGrounded appropriately
        //if (other.collider.CompareTag("Ground"))
        //{
        //    isGrounded = true;
        //}
        if (other.collider.CompareTag("BubbleBouncer"))
        {
            if (!other.gameObject.GetComponentInParent<PlayerBubbleWalker>().hasCrab)
            {
                BubbleBounce(false);
            }
            else
            {
                BubbleBounce(true);
            }
            print(other.gameObject.GetComponentInParent<PlayerBubbleWalker>().hasCrab);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //if (other.collider.CompareTag("Ground"))
        //{
        //    isGrounded = false;
        //}
        if (other.collider.CompareTag("BubbleBouncer"))
        {
            isEmptyBouncing = false;
        }
    }

    public void BubbleBounce(bool isFullBounce)
    {
        if (isFullBounce == false)
        {
            if (speedVertCurrent < emptyBounceHeight)
            {
                isEmptyBouncing = true;
            }
            print("Empty bounced");
        }
        else
        {
            if (speedVertCurrent < fullBounceHeight)
            {
                isFullBouncing = true;
            }
            print("FullBounced");
        }

        return;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) // If Bobby touches an Enemy, transitions to Game Over screen
        {
            SceneManager.LoadScene("Lose");
        }

        if (other.gameObject.CompareTag("WinBound")) // If Bobby touches the goal, transitions to Win screen
        {
            SceneManager.LoadScene("Win");
        }
    }

}
// Made by Wyatt Blackwell
// Not from any specific tutorial
// Edited by (name), edited to do (thing)
// Controls bubble physics, absorbing enemies, showing absorbed enemies, movement direction, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBubbleWalker : MonoBehaviour
{
    private Rigidbody2D rb;
    [Tooltip("The hitbox the player jumps off of.")]
    public Rigidbody2D platform;
    private Vector2 moveDirection; // Direction the bubble is moving in
    private Transform bubbleTrans;
    private int horizVel; // was it shot facing left or right?
    [Tooltip("The model used when bubble hits a crab")]
    public GameObject crabModel;
    [HideInInspector] public bool hasCrab;
    //private PlayerBubbleBounce bounceScript;

    //Audio clips stored in variables
    public AudioSource hitAudio;
    public AudioSource popAudio;
    public AudioSource popCrabAudio;

    [Tooltip("The speed of the bubble")]
    public float bubbleSpeed;
    [Tooltip("How fast the bubble decelerates. Should probably be kept above .96 at a minimum!")]
    public float deceleration;
    [Tooltip("How hard the bubbel decelerates when bubbling a crab.")]
    public float crabDecel;
    [Tooltip("How long until bubble can be jumped on?")]
    public float timeUntilJumpable;

    //public GameObject thinker;
    //public VictoryCheck vicCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        platform = GetComponentInChildren<Rigidbody2D>(); // Pl
        bubbleTrans = rb.transform;

        if (bubbleTrans.rotation.y <= -180) { // Storing the horizontal firing direction in the rotation. -1 is left, 1 is right.
            horizVel = -1;
        }
        else {
            horizVel = 1;
        }
        
        //vicCheck = thinker.GetComponent<VictoryCheck>();

        bubbleTrans.rotation.Set(0, 0, 0, 0); //Reset rotation.


        StartCoroutine(popper());
        
        Destroy(gameObject, 6); // Destroy after 5 seconds
        
    }
    
    IEnumerator popper()
    {
        yield return new WaitForSeconds(5);
        GetComponent<Renderer>().enabled = false;
        crabModel.SetActive(false);
        if (hasCrab == true)
        {
            popCrabAudio.Play();

        }
        else
        {
            popAudio.Play();
        }
        //Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            moveDirection = bubbleTrans.right * horizVel;
            rb.velocity = (moveDirection * bubbleSpeed);

            if (bubbleSpeed > .1f)
            {
                bubbleSpeed *= deceleration; // decelerate the bubble.
            }
            else 
            {
                bubbleSpeed = 0; // If slow enough, just halt it.
            }
            //Debug.Log("BubbleTrans.forward: " + bubbleTrans.forward);
        }
    }


    //Update is called once per frame
    //void Update()
    //{

    //}

    [Tooltip("Function to destroy bubble,")]
    public void DestroyBubble() // Destroy bubble. 
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && hasCrab != true) // If it hits enemy and we don't have a crab, absorb it.
        {
            other.gameObject.GetComponent<CrabWalker>().Bubbled();
            hasCrab = true; // We now have a crab
            crabModel.SetActive(true); // Turn on the model
            bubbleSpeed *= crabDecel; // Cut the speed
            //vicCheck.killCount += 1;
            hitAudio.Play();

            // Make the bubble a bit bigger.
            gameObject.transform.localScale.Set(gameObject.transform.localScale.x * 1.15f, gameObject.transform.localScale.y * 1.15f, gameObject.transform.localScale.z * 1.15f);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class P_BubbleWalker : MonoBehaviour
{
    //debug


    private Rigidbody2D rb;
    public Rigidbody2D platform;
    private Vector2 moveDirection; // Direction the bubble is moving in
    private Transform bubbleTrans;
    private int horizVel; // was it shot facing left or right?

    [Tooltip("The speed of the bubble")]
    public float bubbleSpeed;
    [Tooltip("How fast the bubble decelerates. Should probably be kept above .97 at a minimum!")]
    public float deceleration;
    [Tooltip("How long until it becomes platform?")]
    public float timeToPlatform;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        platform = GetComponentInChildren<Rigidbody2D>();
        bubbleTrans = rb.transform;

        if (bubbleTrans.rotation.y <= -180) // Storing the horizontal firing direction in the rotation. It's something!
        {
            horizVel = -1;
        }
        else
        {
            horizVel = 1;
        }
        //rb.velocity = (bubbleTrans.forward * 5);

        Destroy(gameObject, 5); // Destroy after 5 seconds
    }
    
    void FixedUpdate()
    {
        if (rb != null)
        {
            moveDirection = bubbleTrans.right * horizVel;
            rb.velocity = (moveDirection * bubbleSpeed);

            if (bubbleSpeed > .1f)
            {
                bubbleSpeed *= deceleration;
            }
            else 
            {
                bubbleSpeed = 0;
            }
            //Debug.Log("BubbleTrans.forward: " + bubbleTrans.forward);
        }
    }



    //Update is called once per frame
    void Update()
    {
        if (timeToPlatform <= 0f)
        { 
            platform.simulated = true;
            Debug.Log(platform.simulated);
        }
        else
        {
            timeToPlatform -= Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BubbleWalker : MonoBehaviour
{
    //debug


    private Rigidbody2D rb;
    private Vector2 moveDirection; // Direction the bubble is moving in
    private Transform bubbleTrans;

    [Tooltip("The speed of the bubble")]
    public float bubbleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        bubbleTrans = rb.transform;
        //rb.velocity = (bubbleTrans.forward * 5);

        Destroy(gameObject, 5); // Destroy after 5 seconds
    }
    
    void FixedUpdate()
    {
        if (rb != null)
        {
            moveDirection = bubbleTrans.right;
            rb.velocity = (moveDirection * bubbleSpeed);

            if (bubbleSpeed > .1f)
            {
                bubbleSpeed *= 0.98f;
            }
            else 
            {
                bubbleSpeed = 0;
            }
            //Debug.Log("BubbleTrans.forward: " + bubbleTrans.forward);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}

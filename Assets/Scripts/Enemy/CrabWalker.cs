// Made by Wyatt Blackwell
// Not from any specific tutorial
// Edited by (name), edited to do (thing)
// Controls the crab and crab related accessories.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CrabWalker : MonoBehaviour
{
    [Tooltip("How fast the crab walks")]
    public float speed;
    private Vector2 movement;
    //[Tooltip("Does crab use a pre-specified walk distance? Not functional!")]
    //public bool usesManualWalkDistance;
    //[Tooltip("If enabled, the manual walk distance. Not functional!")]
    //public float walkDistance;
    [Tooltip("Left-side raycast point for ensuring crab does not walk off of ledge. Is the ONLY raycast point despite the name!")]
    public Transform raycastPointLeft;
    private LayerMask emptyMask; // Raycast layer mask
    private LayerMask groundMask;

    private Rigidbody2D rb;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        emptyMask = LayerMask.GetMask("Default", "SceneViewOnly");


        speed *= 60; // Crab moves w/ delta time, adjust speed to match 60hz physics.

        
    }

    // Update is called once per frame
    void Update()
    {
        movement.Set (speed * Time.deltaTime, rb.velocity.y); // set movement vector, leave previous y value to let gravity do it's thing.
        rb.velocity = movement; // set velocity of crab to the movement vector
        
    }

    private void FixedUpdate()
    {
        // Raycast to detect empty ground — adjust this number to alter it's length ------------------------\/
        if (Physics2D.Raycast(raycastPointLeft.position, raycastPointLeft.TransformDirection(Vector2.down), 1.3f, emptyMask) != true)
        {
            speed *= -1; // Flip movement
            print("Crab Rotated!");
            raycastPointLeft.RotateAround(rb.position, Vector3.up, 180); // Spin raycast point 180 degrees. That's right, we're only checking from one.
        }
        else if (Physics2D.Raycast(raycastPointLeft.position, raycastPointLeft.TransformDirection(Vector2.zero), .01f) == true)
        {
            speed *= -1;
            print("Crab Wall Rotated!");
            raycastPointLeft.RotateAround(rb.position, Vector3.up, 180);
        }
    }

    public void Bubbled() // Public function to destroy self.
    {
        Destroy(gameObject);
    }


    // Debug when touching bubble
    //public void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("BubbleShot"))
    //    {
    //        print("Bubbled!");
    //    }
    //}

} // End of script
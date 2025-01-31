using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Collider2D detector;
    public PlayerWalker walker;

    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponent<Collider2D>();
        walker = gameObject.GetComponentInParent<PlayerWalker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            walker.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            walker.isGrounded = false;
        }
    }
}

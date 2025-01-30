// Made by Wyatt Blackwell
// Not from any specific tutorial
// Edited by (name), edited to do (thing)
// One of two scripts that attempts to communicate that the player should bounce off of a bubble.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public PlayerBubbleWalker bubble;

    //Start is called before the first frame update
    void Start()
    {
        bubble = GetComponentInParent<PlayerBubbleWalker>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LowerBouncer")) {
            //other.gameObject.GetComponent<PlayerWalker>().isBouncing = true;
            other.gameObject.GetComponentInParent<PlayerWalker>().BubbleBounce(bubble.hasCrab);
            //walker.isBouncing = true;
            Pop();
            Debug.Log("Playerbounce triggered on: " + other.gameObject.name);
            
        }
    }

    private void Pop()
    {
        Destroy(bubble.gameObject);
    }
}
// Made by Wyatt Blackwell
// Not from any specific tutorial
// Edited by (name), edited to do (thing)
// One of two scripts that attempts to communicate that the player should bounce off of a bubble.

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerBubbleBounce : MonoBehaviour
{
    //private PlayerBubbleWalker bubble;
    public PlayerWalker walker;

    //Start is called before the first frame update
    //void Start()
    //{
        //bubble = GetComponentInParent<PlayerBubbleWalker>();
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BubbleBouncer")) {
            walker.BubbleBounce(other.gameObject.GetComponentInParent<PlayerBubbleWalker>().hasCrab);
            //walker.isBouncing = true;
            Debug.Log("Player bubble bounce triggered on: " + other.gameObject.name);
        }
    }
}
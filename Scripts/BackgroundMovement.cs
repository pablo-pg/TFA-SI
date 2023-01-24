using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour {
    /// <summary>
    /// The complete game object of the second image
    /// </summary>
    public GameObject background1;

    /// <summary>
    /// The complete game object of the second image
    /// </summary>
    public GameObject background2;

    /// <summary>
    /// The speed of the background movement
    /// </summary>
    public float speed;

    /// <summary>
    /// How many pixels to jump
    /// </summary>
    private float jumpPostion;

    /// <summary>
    /// The maximum position to the left of the images to jump
    /// </summary>
    public float leftLimit = -50;

    /// <summary>
    /// The position, rotation and scale of the first background
    /// </summary>
    Transform posBackground1;

    /// <summary>
    /// The position, rotation and scale of the second background
    /// </summary>
    Transform posBackground2;


    /// <summary>
        /// Get the size of the image and compute how many pixels it has to jump
    /// </summary>
    void Start() {
        jumpPostion = background1.GetComponent<SpriteRenderer>().bounds.size.x * 2;
        posBackground1 = background1.GetComponent<Transform>();
        posBackground2 = background2.GetComponent<Transform>();
    }

    /// <summary>
        /// Moves the background to the left and if it exceeds a position it jumps to the right
    /// </summary>
    void LateUpdate() {
        posBackground1.position += Vector3.left * speed * Time.deltaTime;
        if (posBackground1.position.x < leftLimit) {
            var pos = posBackground1.position + Vector3.right * jumpPostion;
            posBackground1.position = pos;
        }

        posBackground2.position += Vector3.left * speed * Time.deltaTime;
        if (posBackground2.position.x < leftLimit) {
            var pos = posBackground2.position + Vector3.right * jumpPostion;
            posBackground2.position = pos;
        }
    }
}

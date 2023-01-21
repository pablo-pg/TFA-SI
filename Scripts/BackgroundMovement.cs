using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public GameObject background1;
    public GameObject background2;
    public float speed;
    float jumpPostion = 50;
    public float leftLimit = -10;
    Transform posBackground1;
    Transform posBackground2;

    void Start() {
        jumpPostion = background1.GetComponent<SpriteRenderer>().bounds.size.x * 2;
        posBackground1 = background1.GetComponent<Transform>();
        posBackground2 = background2.GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
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

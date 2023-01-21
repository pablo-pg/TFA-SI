using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class Flappy : MonoBehaviour {
    /// <value name="jumpSpeed"> Float which indicates the jump speed </value>
    public float jumpSpeed;

    private Rigidbody2D rb;

    /// <summary>
        /// boot function in which  the rigidbody of our blooper is collected
    /// </sumary>
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <value name="jumpRequest"> Lets you know if the blooper should jump or not </value>
    public bool jumpRequest;

    /// <summary>
        /// Update function we rotate the orientation of the blooper when he jumps.
        /// it depends on the speed.
    /// </sumary>
    private void Update() {
        if (rb.velocity.y > 0) {
            transform.eulerAngles = new Vector3(0, 0, 45);
        } else if (rb.velocity.y < 0) {
            transform.eulerAngles = new Vector3(0, 0, -45);
        } else {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
        /// Check if the blooper wants to jump. If a jump is requested, the
        /// blooper jumps (adding an up force on the rigidbody).
    /// </sumary>
    private void FixedUpdate() {
        if (!jumpRequest) return;
        jumpRequest = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }
}

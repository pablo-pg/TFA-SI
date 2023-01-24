using UnityEngine;

public class Obstacles : MonoBehaviour {
    [Tooltip("The speed at which the obstacles move")]
    [SerializeField] private float speed;

    [Tooltip("The x position where the obstacles will be spawned")]
    [SerializeField] private float xDistance = 15;

    public Transform center;

    /// <value> Representing the possible 'Y' positions. </value>
    private Vector2 yRange = new Vector2(5, -1.3f);

    /// <value> The initial position. </value>
    private Vector3 startPoint;

    /// <summary>
        /// Set the start point of the obstacles.
    /// </summary>
    private void Start() {
        startPoint = transform.position;
    }

    /// <summary>
        /// Move the obstacles to the left and reset their position when they reach the end.
    /// </summary>
    private void Update() {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x >= -6) return;

        Vector3 newPosition = transform.position + Vector3.right * xDistance;
        newPosition.y = Random.Range(yRange.x, yRange.y);
        transform.position = newPosition;
    }

    /// <summary>
        /// Reset the position of the obstacles.
    /// </summary>
    public void ReturnToStart() {
        transform.position = startPoint;
    }
}

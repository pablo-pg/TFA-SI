using UnityEngine;
using EvolutionaryPerceptron;

[RequireComponent (typeof (Flappy))]
public class NeuralFlappy : BotHandler {

    /// <value name="showRays"> Boolean that give the information of the ray-cast if is true </value>
    public bool showRays;

    /// <value name="maxDistance"> Maximum distance that the blooper could see </value>
    public float maxDistance;

    /// <value name="blooper"> Object that is going to interact with the interface </value>
    private Flappy blooper;

    private int inputSize;

    private double[, ] lastInputs;

    /// <value name="inputs"> distance obstacles, and the position of the blooper </value>
    private double[, ] inputs;

    private double[, ] ProcessInputs (double[, ] inputs, double time) {
        var currentInput = new double[1, inputSize]; // Sensor info
        for (var i = 0; i < inputSize / 2; i++) {
            currentInput[0, i] = inputs[0, i];
        }

        for (var i = 0; i < inputSize / 2; i++) {
            currentInput[0, i + inputSize / 2] = (currentInput[0, i] - lastInputs[0, i]) * time;
        }

        lastInputs = (double[, ]) currentInput.Clone ();

        return currentInput;
    }

    /// <summary>
        /// Check if the ray hit an obstacle
    /// </summary>
    private bool rayHitObstacle (RaycastHit2D ray) {
        return (ray.collider != null) && !(!ray.collider.CompareTag("Obstacle"));
    }

    /// <summary>
        /// Shot a ray-cast and check if the ray hit an 'Obstacle'.
    /// </summary>
    /// <param name='direction'> The direction in which the ray will be shot. </param>
    /// <param name='obstacles'> A reference to the previously hit obstacles </param>
    /// <returns> The distance to the hit obstacle (max distance posible if it was not hit)</returns>
    private float getDistanceToObstacleHit(Vector2 direction, ref Obstacles obstacles) {
        RaycastHit2D ray = Physics2D.Raycast (transform.position, direction, maxDistance);
        float distanceToObstacle = ray.collider != null ? ray.distance : maxDistance;

        if (showRays) Debug.DrawRay (transform.position, direction * distanceToObstacle);

        if ((obstacles == null) && rayHitObstacle(ray)) obstacles = ray.collider.GetComponentInParent<Obstacles>();
        return distanceToObstacle;
    }


    /// <summary>
        /// Obtain the distance to the obstacles located at the right, up right and down right.
        /// Also gets the position of the blooper and the position of the obstacle hit.
    /// </summary>
    private double[, ] GetInputs () {
        Obstacles o = null;

        float n1 = getDistanceToObstacleHit(Vector2.right, ref o);
        float n2 = getDistanceToObstacleHit((Vector2.right + Vector2.down).normalized, ref o);
        float n3 = getDistanceToObstacleHit ((Vector2.right + Vector2.up).normalized, ref o);
        float n4 = transform.position.y;
        float n5 = o == null ? 0 : o.center.position.y;
        return new double[1, 5] { { n1, n2, n3, n4, n5 } };
    }

    /// <summary>
        /// Some parameters are initialized here
    /// </summary>
    protected override void Start () {
        base.Start ();
        blooper = GetComponent<Flappy> ();
        inputSize = 10;
        lastInputs = new double[1, inputSize];
    }

    /// <summary>
        /// This function update some valors of different parameters
    /// </summary>
    private void Update () {
        var time = Time.deltaTime;
        inputs = GetInputs ();
        inputs = ProcessInputs (inputs, time);
        var output = nb.SetInput (inputs);

        if (output[0, 0] > 0.5f) blooper.jumpRequest = true;

        nb.AddFitness (time);
    }

    /// <summary>
        /// Check if the blooper crashes with some coral (Obstacle).
        /// The blooper disappear if so.
    /// </summary>
    private void OnTriggerEnter2D (Collider2D collision) {
        if (!collision.CompareTag ("Obstacle")) return;
        nb.Destroy ();
    }
}

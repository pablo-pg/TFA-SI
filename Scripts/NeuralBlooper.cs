using UnityEngine;
using EvolutionaryPerceptron;

[RequireComponent (typeof (Blooper))]
public class NeuralBlooper : BotHandler {

    /// <value name="showRays"> Boolean that give the information of the raycast if is true </value>
    public bool showRays;

    /// <value name="maxDistance"> Maximum distance that the blooper could see </value>
    public float maxDistance;

    /// <value name="blooper"> Object that is going to interact with the interface </value>
    private Blooper blooper;

    private int inputSize;

    private double[, ] lastInputs;

    /// <value name="inputs"> distance obstacles, and the position of the blooper </value>
    private double[, ] inputs;

    private double[, ] ProcessInputs(double[, ] inputs, double time) {
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
    private bool rayHitObstacle(RaycastHit2D ray) {
        return (ray.collider != null) && !(!ray.collider.CompareTag("Obstacle"));
    }

    /// <summary>
        /// Shot a raycast and check if the ray hit an 'Obstacle'.
    /// </summary>
    /// <param name='direction'> The direction in which the ray will be shot. </param>
    /// <param name='obstacles'> A reference to the previously hitted obstacles </param>
    /// <returns> The distance to the hitted obstacled (max distance posible if it was not hitted)</returns>
    private float getDistanceToObstacleHitted(Vector2 direction, ref Obstacles obstacles) {
        RaycastHit2D ray = Physics2D.Raycast (transform.position, direction, maxDistance);
        float distanceToObstacle = ray.collider != null ? ray.distance : maxDistance;

        if (showRays) Debug.DrawRay (transform.position, direction * distanceToObstacle);

        if ((obstacles == null) && rayHitObstacle(ray)) obstacles = ray.collider.GetComponentInParent<Obstacles>();
        return distanceToObstacle;
    }


    /// <summary>
        /// Obtain the distance to the obstacles located at the right, up right and down right.
        /// Also gets the position of the blooper and the position of the obstacle hitted.
    /// </summary>
    private double[, ] GetInputs () {
        Obstacles obst = null;

        float n1 = getDistanceToObstacleHitted(Vector2.right, ref obst);
        float n2 = getDistanceToObstacleHitted((Vector2.right + Vector2.down).normalized, ref obst);
        float n3 = getDistanceToObstacleHitted ((Vector2.right + Vector2.up).normalized, ref obst);
        float n4 = transform.position.y;
        float n5 = obst == null ? 0 : obst.center.position.y;
        return new double[1, 5] { { n1, n2, n3, n4, n5 } };
    }

    /// <sumary>
        /// Some parameters are initialized here
    /// </sumary>
    protected override void Start () {
        base.Start ();
        blooper = GetComponent<Blooper> ();
        inputSize = 10;
        lastInputs = new double[1, inputSize];
    }

    /// <sumary>
        /// This function update some valors of diferent parameters
    /// </sumary>
    private void Update () {
        var time = Time.deltaTime;
        inputs = GetInputs();
        inputs = ProcessInputs(inputs, time);
        var output = nb.SetInput(inputs);

        if (output[0, 0] > 0.5f) blooper.jumpRequest = true;  //< Si es mas de 0.5, mejor saltar

        nb.AddFitness(time);
    }

    /// <sumary>
        /// Check if the blooper crashs with some coral (Obstacle).
        /// The blooper disappear if so.
    /// </sumary>
    private void OnTriggerEnter2D (Collider2D collision) {
        if (!collision.CompareTag ("Obstacle")) return;
        nb.Destroy ();
    }
}

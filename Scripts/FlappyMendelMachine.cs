using System.Collections;
using UnityEngine;

using EvolutionaryPerceptron.MendelMachine;

public class FlappyMendelMachine : MendelMachine {
    [Header ("FlappyStuff")]

    /// <summary>
    /// The position where the population borns
    /// </summary>
    public Transform startPoint;

    /// <summary>
    /// The game obstacles
    /// </summary>
    public Obstacles[] obstacles;

    /// <summary>
    /// The population size
    /// </summary>
    private int index = 15;

    /// <summary>
    /// The event variable that delete the actual score to reset it
    /// </summary>
    public delegate void Message();

    /// <summary>
    /// The message that delete the actual score to reset it
    /// </summary>
    public Message onGenerateDeath;

    /// <summary>
    /// Generate the population and start the initial corutine
    /// </summary>
    protected override void Start () {
        base.Start ();
        StartCoroutine(InstantiateBotCoroutine());
    }

    /// <summary>
    /// Overrides the method. Calls the parent method and then destroy the gameobject.
    /// If there isn't any bots, restart the corutine and the actual score
    /// </summary>
    /// <param name="neuralBot">The bot that touched a wall</param>
    public override void NeuralBotDestroyed(Brain neuralBot) {
        base.NeuralBotDestroyed(neuralBot);
        Destroy(neuralBot.gameObject);
        index--;
        if (index <= 0) {
            Save();
            population = Mendelization();
            generation++;

            StartCoroutine(InstantiateBotCoroutine());
            onGenerateDeath();
        }
    }

    /// <summary>
    /// Corutine that generates the population
    /// </summary>
    /// <returns>Is the point where execution pauses and resumes in the following frame</returns>
    private IEnumerator InstantiateBotCoroutine () {
        yield return null;
        index = individualsPerGeneration;

        for (int i = 0; i < obstacles.Length; i++) {
            obstacles[i].ReturnToStart();
        }

        for (int i = 0; i < population.Length; i++) {
            InstantiateBot(population[i], 999999, startPoint, i);
        }
    }
}


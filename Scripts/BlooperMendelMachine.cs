using System.Collections;
using UnityEngine;

using EvolutionaryPerceptron.MendelMachine;

public class BlooperMendelMachine : MendelMachine {
    [Header ("BlooperStuff")]

    /// <value> The position where the population borns </value>
    public Transform startPoint;

    /// <value> The game obstacles </value>
    public Obstacles[] obstacles;

    /// <value> The population size </value>
    private int index = 15;

    /// <value> The event variable that delete the actual score to reset it </value>
    public delegate void Message();

    /// <value> The message that delete the actual score to reset it </value>
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
        if (index > 0) return;
        Save();
        population = Mendelization();
        generation++;

        StartCoroutine(InstantiateBotCoroutine());
        onGenerateDeath();
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

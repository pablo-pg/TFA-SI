using UnityEngine;
using TMPro;

class ScoreSystem : MonoBehaviour {
    [Tooltip("The notifier that will tell us when to reset the score")]
    [SerializeField] private FlappyMendelMachine notifier;

    [Tooltip("The text that displays the score")]
    [SerializeField] private TMP_Text _scoreText;

    /// <value> How much time has passed since the last pipe. </value>
    float currentTime = 0.0f;

    /// <value> The current score of the gulp. </value>
    private int _score = 0;

    /// <value> The highest score gulp ever reached. </value>
    public static int maxScore = 0;

    /// <summary>
        /// Set the text of the score
    /// </summary>
    private void setText() {
      _scoreText.text = "Score: " + _score;
      _scoreText.text += "\nMax Score: " + maxScore;
    }

    /// <summary>
        /// Initialize the score to 0
    /// </summary>
    private void Start() {
        _score = 0;
        setText();
        notifier.onGenerateDeath += ResetScore;
    }

    /// <summary>
        /// Update the score every 2 seconds
    /// </summary>
    private void Update() {
      currentTime += Time.deltaTime;
      if (currentTime > 1.675) {
        _score++;
        currentTime = 0.0f;
      }

      if (_score > maxScore) maxScore = _score;
      setText();
    }

    /// <summary>
        /// Reset the score to 0
    /// </summary>
    public void ResetScore() {
        _score = 0;
        currentTime  =  0.0f;
    }
}

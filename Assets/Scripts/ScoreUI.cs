using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUI : MonoBehaviour
{
      [SerializeField]
      public Text scoreText; // Reference to the Text component for displaying the score

    void Start()
    {
        // Ensure the score text is initialized properly
        if (scoreText == null)
        {
            Debug.LogError("Score text component is not assigned!");
        }
        else
        {
            // Set initial score text
            scoreText.text = "Score: 0";
        }
    }

    // Method to update the score text
    public void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + newScore.ToString();
        }
    }
}

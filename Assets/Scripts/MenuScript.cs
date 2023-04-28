using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // We want to be able to modify the text within the "Difficulty" button
    // so we need to store a reference to it
    // We'll be using TMPro, so we need to import it
    public TMPro.TextMeshProUGUI difficultyText;

    private enum Difficulty { Easy, Medium, Hard };
    private Difficulty difficulty = Difficulty.Easy;

    public void PlayGame() {
        // Load the game scene
        PlayerPrefs.SetString("difficulty", ((int) difficulty).ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeDifficulty() {
        // Change the difficulty
        switch (difficulty) {
            case Difficulty.Easy:
                difficulty = Difficulty.Medium;
                break;
            case Difficulty.Medium:
                difficulty = Difficulty.Hard;
                break;
            case Difficulty.Hard:
                difficulty = Difficulty.Easy;
                break;
        }
        // Update the text
        difficultyText.text = "Difficulty: " + difficulty.ToString();
    }

    public void QuitGame() {
        // Quit the game
        Application.Quit();
    }
}

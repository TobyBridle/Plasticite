using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    // We want to be able to modify the text displayed on the death screen
    // so we need to store a reference to it
    public TMPro.TextMeshProUGUI deathText;

    private void Start() {

    }

    public void Display() {
        int wave = Mathf.Max(PlayerPrefs.GetInt("wave"), 1);
        int difficulty = PlayerPrefs.GetInt("difficulty");
        string difficultyString = "";
        switch (difficulty) {
            case 0:
                difficultyString = "Easy";
                break;
            case 1:
                difficultyString = "Medium";
                break;
            case 2:
                difficultyString = "Hard";
                break;
        }
        deathText.text = "You made it to wave " + wave + " on " + difficultyString + " difficulty!";
        gameObject.SetActive(true);
    }

    public void Restart() {
        // Load the game scene again
        // and use the same difficulty
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        // Quit the game
        Application.Quit();
    }
}

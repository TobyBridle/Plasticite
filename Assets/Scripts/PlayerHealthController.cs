using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public HealthUIController controller;
    // We need a reference to the Rigidbody2D component
    private Rigidbody2D rb;
    // We want to take a reference to a canvas
    // which will be displayed when the player dies
    public DeathScreen deathScreen;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        controller.setMaxHealth(100);
        rb = GetComponent<Rigidbody2D>();
        controller.onDeath(onDeath);
    }

    private void onDeath() {

        audioSource.Play();

        // We want to display the death screen
        // and remove the player from the scene
        deathScreen.Display();
        // Destroy the cursor being used by the player
        Destroy(GameObject.Find("Cursor"));
        Destroy(gameObject);
        Cursor.visible = true;

        // We want to reset the player's stats
        // so that the player can start a new game
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        playerStats.resetStats();
        PlayerPrefs.DeleteAll();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth { get; private set; }
    // current health should be public get, private set
    public int currentHealth { get; private set;}

    private System.Action deathCallback;

    public Slider healthBar; // 0 - maxHealth

    private int gracePeriod = 500; // 500ms invincibility after spawning
    private bool gracePeriodEnded = false;

    private void Start() {
        StartCoroutine(GracePeriod());
    }

    public void setMaxHealth(int maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    /**
     * Take damage and return true if the object is dead
     */
    public bool takeDamage(int damage) {
        if (!gracePeriodEnded) return false;
        if (currentHealth <= 0) {
            if (deathCallback != null)
                deathCallback();
            return true;
        } else if (currentHealth - damage <= 0) {
            currentHealth = 0;
            healthBar.value = 0;
            deathCallback();
            return true;
        }
        currentHealth -= damage;
        healthBar.value = currentHealth;
        return false;
    }

    public void respawn() {
        currentHealth = maxHealth;
        healthBar.value = maxHealth;
    }

    // We want a function which takes
    // a callback function as a parameter
    public void onDeath(System.Action callback) {
        deathCallback = callback;
    }

    private IEnumerator GracePeriod() {
        yield return new WaitForSeconds(gracePeriod / 1000f);
        gracePeriodEnded = true;
    }
}

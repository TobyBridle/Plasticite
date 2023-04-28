using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int projectileDamage { get; private set; }
    public int projectileSpeed { get; private set; }
    public int projectilePenetration { get; private set; }
    private float angle;

    private bool hasPenetrated = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Since the player can upgrade the projectile's stats
        // we want to get the values from the player's stats
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        projectileDamage = playerStats.projectileDamage;
        projectileSpeed = playerStats.projectileSpeed;
        projectilePenetration = playerStats.projectilePenetration;
        
        angle = calculateAngle();

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    void Update()
    {
       Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * projectileSpeed;
       GetComponent<Rigidbody2D>().velocity = velocity; 

        // Destroy if out of screen
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == null) {
            return;
        }

        // If the projectilePenetration is almost half of the enemy's health
        // we want to randomly decide if the projectile should be destroyed
        int otherMaxHealth = other.gameObject.GetComponent<HealthUIController>()?.maxHealth ?? 0;
        float threshold = 0.1f * otherMaxHealth;
        if (otherMaxHealth - threshold < projectilePenetration && projectilePenetration < otherMaxHealth / 2) {
            if (Random.Range(0, 2) == 0) {
                Destroy(gameObject);
            }
        // If the projectilePenetration is not atleast half of the enemy's health
        // we want to destroy the projectile
        } else if (projectilePenetration < otherMaxHealth / 2 || hasPenetrated) {
            Destroy(gameObject);
        }

        // If the projectile collides with an enemy
        // we want to deal projectileDamage to the enemy
        // and then destroy the projectile
        if (other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<HealthUIController>().takeDamage(projectileDamage);
            hasPenetrated = true;
        }
    }

    private float calculateAngle() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2) transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }
}

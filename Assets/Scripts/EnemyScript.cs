using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public float speed, health;
    public int damage;
    private Transform _player;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private bool prevDirection = true; // false = left, true = right
    private bool canDealDamage = true;

    private HealthUIController _health;
    private AudioSource audioSource;


    void Start() {
        audioSource = GetComponent<AudioSource>();

        // We want to get the player object using the tag
        while (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<HealthUIController>();
        _health.setMaxHealth((int) health);
        _health.onDeath(() => {
            audioSource.Play();
            Destroy(gameObject);
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (_player) {
            _direction = (_player.position - transform.position).normalized;
        }
    }

    private void FixedUpdate() {
        if (_player) {
            // We want to move the enemy towards the player
            _rb.velocity = new Vector3(_direction.x * speed, _direction.y * speed, 0);
            // Flip the sprite if necessary (excluding the health bar)
            if (_rb.velocity.x > 0 && prevDirection) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                prevDirection = false;
            } else if (_rb.velocity.x < 0 && !prevDirection) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                prevDirection = true;
            }
        }
    }

    // We want to check if the enemy has collided with the player
    // If there is a collision, we want to deal damage to the player
    // and wait a second before we can deal damage again
    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && canDealDamage) {
            collision.gameObject.GetComponent<PlayerHealthController>().controller.takeDamage(damage);
            StartCoroutine(waitForDamage());
        }
    }

    // We want to wait a second before we can deal damage again
    IEnumerator waitForDamage() {
        canDealDamage = false;
        // Wait for 1 second
        yield return new WaitForSeconds(1);
        canDealDamage = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float runAcceleration = 0.1f;
    public float runDeceleration = 0.1f;
    public float velocityPower = 2f;
    public Rigidbody2D rb;
    private Vector3 _scale;
    private Vector3 minScreenBounds;
    private Vector3 maxScreenBounds;
    // Get the Animator component
    private Animator animator;

    private bool isNearHydrant = false;

    private float minIdleSpeed = 0.4f;

    public GameObject ShopContainer;

    // Start is called before the first frame update
    void Start()
    {
        _scale = transform.localScale;
         minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
         maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
         animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        float moveInput = Input.GetAxisRaw("Horizontal");

        #region Run
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? runAcceleration : runDeceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, velocityPower) * Mathf.Sign(speedDiff);

        // Check if we need to flip the sprite
        if (moveInput > 0) {
            transform.localScale = _scale;
        } else if (moveInput < 0) {
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
        }

        float spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        if (transform.position.x + spriteWidth / 2 > maxScreenBounds.x) {
            transform.position = new Vector3(maxScreenBounds.x - spriteWidth / 2, transform.position.y, transform.position.z);
        } else if (transform.position.x - spriteWidth / 2 < minScreenBounds.x) {
            transform.position = new Vector3(minScreenBounds.x + spriteWidth / 2, transform.position.y, transform.position.z);
        }
        rb.AddForce(movement * Vector2.right);

        // If our speed is greater than minIdleSpeed, we want to play the running animation
        if (Mathf.Abs(rb.velocity.x) > minIdleSpeed) {
            animator.SetBool("isRunning", true);
        } else {
            animator.SetBool("isRunning", false);
        }

        // Get animation speed multiplier
        // At constant speed, the animation should play at 1x speed
        // At 0 speed, the animation should play at 0x speed
        // The speed should be exponential
        animator.SetFloat("speed", Mathf.Pow(Mathf.Abs(rb.velocity.x) / moveSpeed, 0.5f));

        #endregion

    }

    private void Update() {
        // If the player is near the hydrant and presses the E key, we want to open the shop screen
        if (isNearHydrant && Input.GetKeyDown(KeyCode.E)) {
            // Stop the scene
            Time.timeScale = 0f;

            ShopContainer.SetActive(true);
            // We want to increase the wave number
            // in PlayerPrefs
            PlayerPrefs.SetInt("wave", Mathf.Max(PlayerPrefs.GetInt("wave"), 1) + 1);
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

    }

    private void OnCollisionStay2D(Collision2D other) {
        // If it is the hydrant, we want to show the text when the player is near it
        // The hydrant does not have a tag but it is named "EnemySpawner"
        if (other.gameObject.name == "EnemySpawner") {
            other.gameObject.GetComponent<Spawner>().showText();
            isNearHydrant = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        // If it is the hydrant, we want to hide the text when the player is not near it
        // The hydrant does not have a tag but it is named "EnemySpawner"
        if (other.gameObject.name == "EnemySpawner") {
            other.gameObject.GetComponent<Spawner>().hideText();
            isNearHydrant = false;
        }
    }
}

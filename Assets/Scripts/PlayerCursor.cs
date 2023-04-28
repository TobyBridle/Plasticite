using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{

    public GameObject cursor;
    private Vector3 mousePos;
    // We want to have references
    // to sprites for different states (e.g hovering over enemy)
    private Sprite defaultSprite;
    public Sprite hoverEnemy;

    public GameObject projectilePrefab;

    private int shootDelay; // Amount of milliseconds between shots
    private bool canShoot = true;
    private PlayerStats playerStats;

    void Start() {
        // Disable the default cursor
        Cursor.visible = false;
        defaultSprite = cursor.GetComponent<SpriteRenderer>().sprite;
        // Get the gameobject with tag PlayerStats
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        shootDelay = playerStats.projectileFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        // We need to convert the mouse position from screen space to world space
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = Camera.main.nearClipPlane;
        // We want to lerp the position of the cursor
        // from its current position to the mouse position
        lerp(cursor.transform.position, mousePos, 0.1f);

        // If the cursor is hovering over an enemy
        // we want to switch the sprite to the hoverEnemy sprite
        // otherwise we want to switch it back to the default sprite
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && hit.collider.tag == "Enemy") {
            cursor.GetComponent<SpriteRenderer>().sprite = hoverEnemy;
            // When the player clicks on the screen, we want to fire a projectile
            // towards the mouse position
            if (Mouse.current.leftButton.wasPressedThisFrame && canShoot) {
                // We want to instantiate a projectile at the player's position
                // and then we want to move it towards the mouse position
                // We need to instantiate the projectile
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                StartCoroutine(shootDelayCoroutine());
            }
        } else {
            cursor.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
    }

    private void lerp(Vector2 start, Vector2 end, float t) {
        cursor.transform.position = Vector2.Lerp(start, end, t);
    }

    IEnumerator shootDelayCoroutine() {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay / 1000);
        canShoot = true;
    }
}

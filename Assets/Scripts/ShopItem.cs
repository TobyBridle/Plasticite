using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{

    public enum ItemType
    {
        ProjectileSpeed,
        ProjectileDamage,
        ProjectilePenetration,
        ProjectileFrequency
    };

    public ItemType itemType;
    public GameObject ShopContainer;

    private PlayerStats playerStats;
    private AudioSource audioSource;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
        int level = 0;
        switch (itemType) {
            case ItemType.ProjectileSpeed:
                level = playerStats.projectileSpeedUpgradeLevel;
                break;
            case ItemType.ProjectileDamage:
                level = playerStats.projectileDamageUpgradeLevel;
                break;
            case ItemType.ProjectilePenetration:
                level = playerStats.projectilePenetrationUpgradeLevel;
                break;
            case ItemType.ProjectileFrequency:
                level = playerStats.projectileFrequencyUpgradeLevel;
                break;
        }

        if (level == 4) {
            // Disable the Button Component (child of this object)
            GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void OnUpgrade() {
        switch (itemType) {
            case ItemType.ProjectileSpeed:
                playerStats.upgradeProjectileSpeed();
                break;
            case ItemType.ProjectileDamage:
                playerStats.upgradeProjectileDamage();
                break;
            case ItemType.ProjectilePenetration:
                playerStats.upgradeProjectilePenetration();
                break;
            case ItemType.ProjectileFrequency:
                playerStats.upgradeProjectileFrequency();
                break;
        }

        audioSource.Play();

        // Disable the Shop Container
        ShopContainer.SetActive(false);

        // Now we want to reload the scene
        // We can do this by getting the current scene name
        // and loading it again
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}

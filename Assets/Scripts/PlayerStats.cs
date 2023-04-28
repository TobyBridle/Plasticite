using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int projectileSpeed {
        get {
            return projectileSpeedUpgrades[projectileSpeedUpgradeLevel];
        }
        private set {}
    }
    public int projectileDamage {
        get {
            return projectileDamageUpgrades[projectileDamageUpgradeLevel];
        }
        private set {}
    }
    public int projectilePenetration {
        get {
            return projectilePenetrationUpgrades[projectilePenetrationUpgradeLevel];
        }
        private set {}
    }
    public int projectileFrequency {
        get {
            return projectileFrequencyUpgrades[projectileFrequencyUpgradeLevel];
        }
        private set {}
    }

    // We want a list of upgrades and what they set the stats to
    private static List<int> projectileSpeedUpgrades = new List<int> { 5, 10, 15, 20, 25 };
    private static List<int> projectileDamageUpgrades = new List<int> { 5, 10, 15, 20, 25 };
    private static List<int> projectilePenetrationUpgrades = new List<int> { 10, 20, 30, 40, 50 };
    private static List<int> projectileFrequencyUpgrades = new List<int> { 700, 600, 500, 400, 300 };

    // We want to keep track of the current upgrade level
    public int projectileSpeedUpgradeLevel {
        get {
            // Get from playerprefs
            return PlayerPrefs.GetInt("projectileSpeedUpgradeLevel", 0);
        }
        private set {
            // Set playerprefs
            PlayerPrefs.SetInt("projectileSpeedUpgradeLevel", value);
        }
    }
    public int projectileDamageUpgradeLevel {
        get {
            // Get from playerprefs
            return PlayerPrefs.GetInt("projectileDamageUpgradeLevel", 0);
        }
        private set {
            // Set playerprefs
            PlayerPrefs.SetInt("projectileDamageUpgradeLevel", value);
        }
    }
    public int projectilePenetrationUpgradeLevel {
        get {
            // Get from playerprefs
            return PlayerPrefs.GetInt("projectilePenetrationUpgradeLevel", 0);
        }
        private set {
            // Set playerprefs
            PlayerPrefs.SetInt("projectilePenetrationUpgradeLevel", value);
        }
    }
    public int projectileFrequencyUpgradeLevel {
        get {
            // Get from playerprefs
            return PlayerPrefs.GetInt("projectileFrequencyUpgradeLevel", 0);
        }
        private set {
            // Set playerprefs
            PlayerPrefs.SetInt("projectileFrequencyUpgradeLevel", value);
        }
    }

    void Start()
    {
        projectileSpeed = projectileSpeedUpgrades[projectileSpeedUpgradeLevel];
        projectileDamage = projectileDamageUpgrades[projectileDamageUpgradeLevel];
        projectilePenetration = projectilePenetrationUpgrades[projectilePenetrationUpgradeLevel];
        projectileFrequency = projectileFrequencyUpgrades[projectileFrequencyUpgradeLevel];
    }

    public void upgradeProjectileSpeed() {
        if (projectileSpeedUpgradeLevel < projectileSpeedUpgrades.Count - 1) {
            projectileSpeedUpgradeLevel++;
            projectileSpeed = projectileSpeedUpgrades[projectileSpeedUpgradeLevel];
        }
    }

    public void upgradeProjectileDamage() {
        if (projectileDamageUpgradeLevel < projectileDamageUpgrades.Count - 1) {
            projectileDamageUpgradeLevel++;
            projectileDamage = projectileDamageUpgrades[projectileDamageUpgradeLevel];
        }
    }

    public void upgradeProjectilePenetration() {
        if (projectilePenetrationUpgradeLevel < projectilePenetrationUpgrades.Count - 1) {
            projectilePenetrationUpgradeLevel++;
            projectilePenetration = projectilePenetrationUpgrades[projectilePenetrationUpgradeLevel];
        }
    }

    public void upgradeProjectileFrequency() {
        if (projectileFrequencyUpgradeLevel < projectileFrequencyUpgrades.Count - 1) {
            projectileFrequencyUpgradeLevel++;
            projectileFrequency = projectileFrequencyUpgrades[projectileFrequencyUpgradeLevel];
        }
    }

    public void resetStats() {
        projectileSpeedUpgradeLevel = projectileDamageUpgradeLevel = projectilePenetrationUpgradeLevel = projectileFrequencyUpgradeLevel = 0;

        projectileSpeed = projectileSpeedUpgrades[projectileSpeedUpgradeLevel];
        projectileDamage = projectileDamageUpgrades[projectileDamageUpgradeLevel];
        projectilePenetration = projectilePenetrationUpgrades[projectilePenetrationUpgradeLevel];
        projectileFrequency = projectileFrequencyUpgrades[projectileFrequencyUpgradeLevel];
    }
}

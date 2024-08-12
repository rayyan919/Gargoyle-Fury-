using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossManager : MonoBehaviour
{
    public int health = 100; // Public health variable
    public Transform gargoyleBarrier; // Public transform for the barrier
    public Transform losePosition; // Public transform for the lose position
    public Transform spawnPosition; // Public transform for the spawn position
    public Transform roof; // Public transform for the roof
    public Transform ceiling; // Public transform for the ceiling
    public TextMeshPro hitMessageText; // Reference to the TextMeshPro text box for hit messages
    public GameObject bossPrefab; // Reference to the boss prefab
    public float initialSpeed = 5f; // Public initial speed

    private GameObject bossInstance;

    void Start()
    {
        // Set the current level in PlayerPrefs
        PlayerPrefs.SetString("CurrentLevel", "Level 5");

        // Instantiate the boss prefab at the spawn position
        bossInstance = Instantiate(bossPrefab, spawnPosition.position, Quaternion.identity);
    }
}

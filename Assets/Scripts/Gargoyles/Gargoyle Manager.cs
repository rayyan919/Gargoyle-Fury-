using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace

public class GargoyleManager : MonoBehaviour
{
    public Transform spawn1, spawn2, spawn3;
    public Transform lose1, lose2, lose3;

    public GameObject brownGargoylePrefab;
    public GameObject blueGargoylePrefab;
    public GameObject greyGargoylePrefab;

    public int brownCount;
    public int blueCount;
    public int greyCount;

    public string levelName;
    public string NextLevel;
    public float spawnDelayMin = 0.5f; // Minimum delay between spawns
    public float spawnDelayMax = 1f;   // Maximum delay between spawns

    public TextMeshPro hitMessageText; // Public variable for TextMeshPro UI

    private Transform[] spawnPositions;
    private Transform[] losePositions;
    private List<GameObject> activeGargoyles;
    private SlingshotNew slingshot; // Reference to the SlingshotNew script

    private int killedGargoyles = 0; // Track the number of killed gargoyles
    private int totalGargoyles; // Total gargoyles to be spawned

    void Start()
    {
        // Get reference to the SlingshotNew script
        slingshot = FindObjectOfType<SlingshotNew>();

        spawnPositions = new Transform[] { spawn1, spawn2, spawn3 };
        losePositions = new Transform[] { lose1, lose2, lose3 };
        activeGargoyles = new List<GameObject>();

        totalGargoyles = brownCount + blueCount + greyCount; // Calculate total gargoyles
        PlayerPrefs.SetString("CurrentLevel", levelName); // Set the current level in PlayerPrefs
        PlayerPrefs.SetString("NextLevel", NextLevel);
        StartCoroutine(SpawnGargoyles());
    }

    void Update()
    {
        // Check if all active gargoyles have been killed
        if (killedGargoyles >= totalGargoyles && activeGargoyles.Count == 0)
        {
            CheckWinCondition();
        }

        // Check if any gargoyle has crossed the losing positions
        foreach (GameObject gargoyle in activeGargoyles)
        {
            if (gargoyle != null)
            {
                CheckLoseCondition(gargoyle);
            }
        }
    }

    IEnumerator SpawnGargoyles()
    {
        while (brownCount > 0 || blueCount > 0 || greyCount > 0)
        {
            Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
            GameObject gargoyle = null;

            int randomGargoyleType = Random.Range(0, brownCount + blueCount + greyCount);

            if (randomGargoyleType < brownCount)
            {
                gargoyle = Instantiate(brownGargoylePrefab, spawnPosition.position, Quaternion.identity);
                brownCount--;
            }
            else if (randomGargoyleType < brownCount + blueCount)
            {
                gargoyle = Instantiate(blueGargoylePrefab, spawnPosition.position, Quaternion.identity);
                blueCount--;
            }
            else
            {
                gargoyle = Instantiate(greyGargoylePrefab, spawnPosition.position, Quaternion.identity);
                greyCount--;
            }

            if (gargoyle != null)
            {
                activeGargoyles.Add(gargoyle);
            }

            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
        }
    }

    public void GargoyleKilled()
    {
        killedGargoyles++;
        activeGargoyles.RemoveAll(g => g == null);

        Debug.Log("Gargoyle killed, Total Killed: " + killedGargoyles);

        // Check the win condition after a gargoyle is killed
        if (killedGargoyles >= totalGargoyles)
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        // Check if all gargoyles have been killed
        if (killedGargoyles >= totalGargoyles)
        {
            Debug.Log("Player wins!");

            // Load the win scene
            if (levelName != "Level 5")
            {
            SceneManager.LoadScene("Level Win");
            }
        }
    }

    void CheckLoseCondition(GameObject gargoyle)
    {
        // Access rockCount and spikeyBallCount from SlingshotNew script
        int totalAmmoValue = (slingshot.rockCount * 10) + (slingshot.spikeyBallCount * 20);
        int totalGargoyleValue = (brownCount * 20) + (blueCount * 30) + (greyCount * 40);

          if (totalAmmoValue < totalGargoyleValue)
        {
            Debug.Log("Not enough ammo to defeat remaining gargoyles. Player loses!");
            SceneManager.LoadScene("Level End");
            return;
        }

        // Check if the gargoyle crosses any lose position or if total ammo value is less than total gargoyle value
        for (int i = 0; i < losePositions.Length; i++)
        {
            if (gargoyle.transform.position.x <= losePositions[i].position.x)
            {
                Debug.Log("Gargoyle crossed lose position!");
                SceneManager.LoadScene("Level End");
                return; // Exit after determining the player has lost
            }
        }
    }
}

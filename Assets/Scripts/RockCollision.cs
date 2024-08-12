using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    
    public GameObject BrownPrefab;
    public GameObject BluePrefab;
    public GameObject GreyPrefab;
    public GameObject BossPrefab;
    

    void OnCollisionEnter2D(Collision2D collision)

    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        // Check if the rock collided with any of the assigned prefabs
        if (collision.gameObject == BrownPrefab ||
            collision.gameObject == BluePrefab ||
            collision.gameObject == GreyPrefab ||
            collision.gameObject == BossPrefab)
        {
            Destroy(gameObject); // Destroy the rock
            Debug.Log("Ammo destroyed");

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GargoyleBehaviour : MonoBehaviour
{
    public float speed; // Speed at which the gargoyle moves
    public int health;  // Health of the gargoyle

    public GameObject PowPrefab;

    public AudioClip soundClip;
    private AudioSource audioSource;


    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Collider2D col; // Reference to the Collider2D component
    private GargoyleManager gargoyleManager; // Reference to the GargoyleManager
    private TextMeshPro hitMessageText; // Reference to the TextMeshPro text box

    // Punchlines for hits and kills
    private string[] hitPunchlines = {
        "That had to hurt!", "Not so tough now, huh?", "Ow!",
        "Ouch!", "Pow!", "That’s gonna leave a mark!",
        "Direct hit!", "Yikes!", "'Rocked' your world!", "Gotcha!"
    };
    
    private string[] killPunchlines = {
        "And stay down!", "One less problem!", 
        "Good riddance!", "That's how it's done!", "Another one bites the dust!","Bullseye!"
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        col.enabled = true; // Ensure the collider is enabled from the start
        rb.velocity = new Vector2(-speed, 0); // Set the gargoyle to move left in the x direction

        // Find the GargoyleManager and get the reference to the hitMessageText from it
        gargoyleManager = FindObjectOfType<GargoyleManager>();
        hitMessageText = gargoyleManager.hitMessageText; // Get the hitMessageText from GargoyleManager
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only process collisions with projectiles
        if (collision.CompareTag("Rock") || collision.CompareTag("SpikeyBall"))
        {
            // Instantiate the "Pow" prefab at the collision position
            GameObject powEffect = Instantiate(PowPrefab, collision.transform.position, Quaternion.identity) as GameObject;
            audioSource.PlayOneShot(soundClip);
            // Destroy the "Pow" prefab after a short delay (0.5 seconds)
            Destroy(powEffect, 0.25f);

            // Reduce health based on projectile type
            if (collision.CompareTag("Rock"))
            {
                health -= 10; // Reduce health by 10 if hit by a rock
            }
            else if (collision.CompareTag("SpikeyBall"))
            {
                health -= 20; // Reduce health by 20 if hit by a spikey ball
            }

            CheckHealth();

            // Destroy the projectile on impact
            Destroy(collision.gameObject);
        }
    }

    void CheckHealth()
    {
        if (health > 0)
        {
            // Display a random hit punchline if the gargoyle is still alive
            DisplayPunchline(hitPunchlines, Color.yellow, Color.black); // Yellow text with black outline for hits
        }
        else
        {
            // Notify GargoyleManager that a gargoyle has been killed
            if (gargoyleManager != null)
            {
                gargoyleManager.GargoyleKilled();
            }

            // Display a random kill punchline if the gargoyle is dead
            DisplayPunchline(killPunchlines, Color.red, Color.black); // Red text with black outline for kills

            // Destroy the gargoyle
            Destroy(gameObject);
        }
    }

    void DisplayPunchline(string[] punchlines, Color textColor, Color outlineColor)
    {
        // Choose a random punchline from the array
        string punchline = punchlines[Random.Range(0, punchlines.Length)];

        // Set the text color and outline color
        hitMessageText.color = textColor;
        hitMessageText.outlineColor = outlineColor;
        hitMessageText.outlineWidth = 0.2f; // Set the outline width (adjust this value as needed)

        // Display the punchline in the TextMeshPro text box
        hitMessageText.text = punchline;

        // Start a coroutine to clear the text after 1 second
        StartCoroutine(ClearTextAfterDelay(0.5f));
    }

    IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hitMessageText.text = ""; // Clear the text
    }
}

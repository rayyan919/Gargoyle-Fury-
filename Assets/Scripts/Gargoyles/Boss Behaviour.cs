using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GargoyleBoss : MonoBehaviour
{
    private BossManager bossManager;
    private Rigidbody2D rb;
    private bool isRetreating = false;
    private bool isCharging = false;
    private bool hasMovedAhead = false;
    public GameObject PowPrefab;
    public AudioClip soundClip;
    private AudioSource audioSource;


    private SlingshotNew slingshotNew;

    private string[] hitPunchlines = {
        "Big hit!", "Gonna feel that!", "Nice shot!",
        "That’s a hit!", "Take that!", "You’ve got me!",
        "Hit me hard!", "Boom!", "You’re on fire!", "Zing!"
    };

    private string[] killPunchlines = {
        "Game over!", "Down and out!", "Victory is yours!",
        "You got me!", "That’s a wrap!", "Finito!",
        "Mission accomplished!", "Done and dusted!", "Goodbye!", "Out of the game!"
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossManager = FindObjectOfType<BossManager>();
        slingshotNew = FindObjectOfType<SlingshotNew>();

        if (bossManager == null || slingshotNew == null)
        {
            Debug.LogError("BossManager or SlingshotNew script not found in the scene.");
        }

        StartCoroutine(MoveAheadFromSpawn());
        StartCoroutine(FluctuateSpeed());
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    void Update()
    {
        if (hasMovedAhead && !isRetreating && !isCharging)
        {
            if (bossManager.health > 20)
            {
                ConfinedMovement();
            }
            else
            {
                StartCoroutine(RetreatAndCharge());
            }
        }

        // Check for the second losing condition: boss reaching losePosition
        if (transform.position.x <= bossManager.losePosition.position.x)
        {
            SceneManager.LoadScene("Level End"); // Trigger level end if the boss reaches losePosition
        }
    }

    IEnumerator MoveAheadFromSpawn()
    {
        rb.velocity = new Vector2(-bossManager.initialSpeed, 0); 

        yield return new WaitUntil(() => 
            transform.position.x < bossManager.spawnPosition.position.x &&
            transform.position.x > bossManager.gargoyleBarrier.position.x);

        hasMovedAhead = true;
        StartConfinedMovement(); // Start moving randomly in the confined area
    }

    void StartConfinedMovement()
    {
        rb.velocity = new Vector2(Random.Range(-bossManager.initialSpeed, bossManager.initialSpeed), 
                                Random.Range(-bossManager.initialSpeed, bossManager.initialSpeed));
    }


    void ConfinedMovement()
    {
        float nextX = transform.position.x + rb.velocity.x * Time.deltaTime;
        float nextY = transform.position.y + rb.velocity.y * Time.deltaTime;

        // Horizontal boundary check
        if (nextX <= bossManager.gargoyleBarrier.position.x || nextX >= bossManager.spawnPosition.position.x)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y); // Reverse X direction
            nextX = Mathf.Clamp(nextX, bossManager.gargoyleBarrier.position.x, bossManager.spawnPosition.position.x); // Clamp X position
        }

        // Vertical boundary check
        if (nextY <= bossManager.ceiling.position.y || nextY >= bossManager.roof.position.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y); // Reverse Y direction
            nextY = Mathf.Clamp(nextY, bossManager.ceiling.position.y, bossManager.roof.position.y); // Clamp Y position
        }

        // Update the Gargoyle's position
        transform.position = new Vector3(nextX, nextY, transform.position.z);

        // Ensure constant speed
        rb.velocity = rb.velocity.normalized * bossManager.initialSpeed;
    }




    IEnumerator FluctuateSpeed()
    {
        while (bossManager.health > 40)
        {
            float newSpeed = Random.Range(3f, 5f); // Only positive speeds, to keep movement smooth
            rb.velocity = new Vector2(newSpeed * Mathf.Sign(rb.velocity.x), rb.velocity.y);
            yield return new WaitForSeconds(1.5f);
        }
    }


    IEnumerator RetreatAndCharge()
    {
        isRetreating = true;

        // Move the boss to the retreat position
        while (transform.position.x < bossManager.spawnPosition.position.x)
        {
            rb.velocity = new Vector2(12f, 0);
            yield return null;
        }

        // Stop all movement and wait for 1 second at the spawn position
        rb.velocity = Vector2.zero;
        yield return null;

        // Calculate direction to the losePosition
        Vector2 directionToLosePosition = (bossManager.losePosition.position - transform.position).normalized;

        isRetreating = false;
        isCharging = true;
        rb.velocity = directionToLosePosition * 12f; // Charge towards the losePosition
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rock") || collision.CompareTag("SpikeyBall"))
        {
            GameObject powEffect = Instantiate(PowPrefab, collision.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(soundClip);
            Destroy(powEffect, 0.25f);

            int damage = collision.CompareTag("Rock") ? 10 : 20;
            bossManager.health -= damage;

            int totalAmmoValue = slingshotNew.rockCount * 10 + slingshotNew.spikeyBallCount * 20;

            // Check if the boss is still alive
            if (bossManager.health > 0)
            {
                DisplayPunchline(hitPunchlines, Color.yellow, Color.black);

                // Check for the first losing condition: ammo is less than boss's remaining health
                if (totalAmmoValue < bossManager.health)
                {
                    SceneManager.LoadScene("Level End"); // Trigger level end if ammo value is less than boss's health
                }
            }
            else
            {
                // If the boss's health is <= 0, the player wins
                DisplayPunchline(killPunchlines, Color.red, Color.black);
                SceneManager.LoadScene("FinalWin"); // Trigger the win scene
            }

            Destroy(collision.gameObject);
        }
    }

    void DisplayPunchline(string[] punchlines, Color textColor, Color outlineColor)
    {
        if (bossManager.hitMessageText == null)
        {
            Debug.LogError("hitMessageText is not assigned.");
            return;
        }

        string punchline = punchlines[Random.Range(0, punchlines.Length)];

        bossManager.hitMessageText.color = textColor;
        bossManager.hitMessageText.outlineColor = outlineColor;
        bossManager.hitMessageText.outlineWidth = 0.2f;

        bossManager.hitMessageText.text = punchline;
        StartCoroutine(ClearTextAfterDelay(0.25f));
    }

    IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bossManager.hitMessageText.text = "";
    }
}
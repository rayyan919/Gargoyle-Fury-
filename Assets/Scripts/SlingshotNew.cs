using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro for UI text

public class SlingshotNew : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;

    public float maxLength;
    public float bottomBoundary;

    bool isMouseDown;

    public GameObject rockPrefab;
    public GameObject spikeyBallPrefab;

    public int rockCount = 5;
    public int spikeyBallCount = 5;

    public float ammoPositionOffset;
    public float force;

    private bool isRock = true; // Track current ammo type
    Rigidbody2D currentAmmo;
    Collider2D currentAmmoCollider;

    // Add TextMeshPro references
    public TextMeshPro rockCountText;
    public TextMeshPro spikeyBallCountText;

    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        UpdateAmmoText();  // Initialize the text with current counts
        CreateAmmo();
    }

    void CreateAmmo()
    {
        if (isRock && rockCount > 0)
        {
            currentAmmo = Instantiate(rockPrefab).GetComponent<Rigidbody2D>();
        }
        else if (!isRock && spikeyBallCount > 0)
        {
            currentAmmo = Instantiate(spikeyBallPrefab).GetComponent<Rigidbody2D>();
        }
        else if (rockCount == 0 && spikeyBallCount > 0)
        {
            isRock = false;
            currentAmmo = Instantiate(spikeyBallPrefab).GetComponent<Rigidbody2D>();
        }
        else if (spikeyBallCount == 0 && rockCount > 0)
        {
            isRock = true;
            currentAmmo = Instantiate(rockPrefab).GetComponent<Rigidbody2D>();
        }
        else
        {
            currentAmmo = null;
            return;
        }

        currentAmmoCollider = currentAmmo.GetComponent<Collider2D>();
        currentAmmoCollider.enabled = false;
        currentAmmo.isKinematic = true;

        ResetStrips();
        UpdateAmmoText();
    }

    void Update()
    {
        if (isMouseDown && currentAmmo != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);

            SetStrips(currentPosition);

            if (currentAmmoCollider)
            {
                currentAmmoCollider.enabled = true;
            }
        }
        else
        {
            ResetStrips();
        }
    }

    private void OnMouseDown()
    {
        if (currentAmmo != null)
        {
            isMouseDown = true;
        }
    }

    private void OnMouseUp()
    {
        if (currentAmmo != null)
        {
            isMouseDown = false;
            Shoot();
            currentPosition = idlePosition.position;
        }
    }

    void Shoot()
    {
        currentAmmo.isKinematic = false;
        Vector3 ammoForce = (currentPosition - center.position) * force * -1;
        currentAmmo.velocity = ammoForce;

        if (isRock && rockCount > 0)
        {
            rockCount--;
        }
        else if (!isRock && spikeyBallCount > 0)
        {
            spikeyBallCount--;
        }

        currentAmmo = null;
        currentAmmoCollider = null;

        if (rockCount == 0 && spikeyBallCount > 0)
        {
            isRock = false;
        }
        else if (spikeyBallCount == 0 && rockCount > 0)
        {
            isRock = true;
        }

        UpdateAmmoText();
        CreateAmmo();  // Create new ammo after the previous one is shot
    }

    public void SwitchAmmoType()
    {
        // Switch the ammo type
        isRock = !isRock;

        // Destroy current ammo and create the new one
        if (currentAmmo != null)
        {
            Destroy(currentAmmo.gameObject);
            CreateAmmo();
        }
    }

    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (currentAmmo)
        {
            Vector3 dir = position - center.position;
            currentAmmo.transform.position = position + dir.normalized * ammoPositionOffset;
            currentAmmo.transform.right = -dir.normalized;
        }
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }

    // Method to update the TextMeshPro components with the current ammo counts
    void UpdateAmmoText()
    {
        rockCountText.text = Mathf.Max(rockCount, 0).ToString();
        spikeyBallCountText.text = Mathf.Max(spikeyBallCount, 0).ToString();
    }
}

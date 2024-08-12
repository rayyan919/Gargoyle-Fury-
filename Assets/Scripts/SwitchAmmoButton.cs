using UnityEngine;

public class SwitchAmmoButton : MonoBehaviour
{
    private SlingshotNew slingshotScript;

    void Start()
    {
        // Find the SlingshotNew script in the scene
        slingshotScript = FindObjectOfType<SlingshotNew>();
    }

    void OnMouseDown()
    {
        if (slingshotScript != null)
        {
            slingshotScript.SwitchAmmoType();
        }
    }
}

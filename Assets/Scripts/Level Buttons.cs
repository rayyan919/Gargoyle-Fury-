using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControls : MonoBehaviour
{

    public string currentLevel;
    
    // Method to restart the current level
    public void Restart()
    {    
        if (!string.IsNullOrEmpty(currentLevel))
        {
            SceneManager.LoadScene(currentLevel);
        }
        else
        {
            Debug.LogError("CurrentLevel not found!");
        }
    }

    // Method to load the Main Menu scene
    public void Home()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("Main Menu");
    }
}

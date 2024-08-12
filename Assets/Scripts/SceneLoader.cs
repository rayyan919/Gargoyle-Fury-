using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public void RetryLevel()
    {
        string levelName = PlayerPrefs.GetString("CurrentLevel");

        if (!string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("No level name found in PlayerPrefs!");
        }
    }

    // public void NextLevel()
    // {
    //     string levelName = PlayerPrefs.GetString("NextLevel");

    //     if (!string.IsNullOrEmpty(levelName))
    //     {
    //         SceneManager.LoadScene(levelName);
    //     }
    //     else
    //     {
    //         Debug.LogError("No next level name found in PlayerPrefs!");
    //     }
    // }

    // Method to load a scene by name
    public void LoadScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        // Retrieve the next level name from PlayerPrefs
        string nextLevelName = PlayerPrefs.GetString("NextLevel");

        // Load the next level
        SceneManager.LoadScene(nextLevelName);
    }
}

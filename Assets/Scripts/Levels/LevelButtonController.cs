using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    public Button levelButton;
    public string levelSceneName;

    void Start()
    {
        if (levelButton != null)
        {
            levelButton.onClick.AddListener(OnLevelButtonClick);
        }
        else
        {
            Debug.LogError("Level button reference is not assigned.");
        }
    }

    public void OnLevelButtonClick()
    {
        if (!string.IsNullOrEmpty(levelSceneName))
        {
            // Load the level scene
            SceneManager.LoadScene(levelSceneName);
        }
        else
        {
            Debug.LogError("Level scene name is not set.");
        }
    }
}

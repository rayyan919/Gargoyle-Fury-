using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    public Button startButton;
    private string levelsSceneName = "Levels";

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
        else
        {
            Debug.LogError("Start button reference is not assigned.");
        }
    }

    void OnStartButtonClick()
    {
        Debug.Log("Start button clicked.");

        if (!string.IsNullOrEmpty(levelsSceneName))
        {
            // Load the levels scene
            SceneManager.LoadScene(levelsSceneName);
        }
        else
        {
            Debug.LogError("Levels scene name is not set.");
        }
    }
}

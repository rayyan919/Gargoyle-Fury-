using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutButton : MonoBehaviour
{
    // Function to open the "About" scene
    public void OpenAbout()
    {
        SceneManager.LoadScene("About");
    }

    // Function to return to the "Main Menu" scene
    public void ReturnAbout()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

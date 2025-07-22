using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SelectLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
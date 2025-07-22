using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void StarterSelect()
    {
        SceneManager.LoadSceneAsync("Starter");
    }

    public void WindingPathSelect()
    {
        SceneManager.LoadSceneAsync("WindingPath");
    }

    public void TheCircleSelect()
    {
        SceneManager.LoadSceneAsync("TheCircle");
    }

    public void SpectaclesSelect()
    {
        SceneManager.LoadSceneAsync("Spectacles");
    }

    public void WastelandSelect()
    {
        SceneManager.LoadSceneAsync("Wasteland");
    }

    public void StraightShotSelect()
    {
        SceneManager.LoadSceneAsync("StraightShot");
    }
}
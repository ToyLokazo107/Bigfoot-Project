using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsPanel;

    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }

        SceneManager.LoadScene("GamePlayKinematic");
    }

    public void OpenOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("ProjectBigfoot");
    }
}
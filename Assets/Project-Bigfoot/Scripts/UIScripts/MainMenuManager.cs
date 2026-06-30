using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsPanel;

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("GamePlayCinematic");
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
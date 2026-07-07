using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsPanel;

    [Header("DOTween Buttons")]
    [SerializeField] private RectTransform startButton;
    [SerializeField] private RectTransform optionsButton;
    [SerializeField] private RectTransform creditsButton;
    [SerializeField] private RectTransform exitButton;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeStatus(GameStatus.MenuPrincipal);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        AnimateButton(startButton, 0f);
        AnimateButton(optionsButton, 0.15f);
        AnimateButton(creditsButton, 0.30f);
        AnimateButton(exitButton, 0.45f);
    }

    private void AnimateButton(RectTransform button, float delay)
    {
        if (button == null) return;

        button.localScale = Vector3.one;

        button.DOScale(1.08f, 0.8f)
            .SetDelay(delay)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);
    }

    private void StopButtonAnimations()
    {
        if (startButton != null) startButton.DOKill();
        if (optionsButton != null) optionsButton.DOKill();
        if (creditsButton != null) creditsButton.DOKill();
        if (exitButton != null) exitButton.DOKill();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        StopButtonAnimations();

        if (startButton != null)
        {
            startButton.DOScale(0.85f, 0.2f).SetEase(Ease.InBack).SetUpdate(true);

            yield return new WaitForSecondsRealtime(0.25f);

            startButton.DOScale(1.2f, 0.4f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        yield return new WaitForSecondsRealtime(2f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }

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
        StopButtonAnimations();
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        StopButtonAnimations();
        Application.Quit();
    }

    public void BackToMenu()
    {
        StopButtonAnimations();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.BackToMenu();
        }
        else
        {
            SceneManager.LoadScene("ProjectBigfoot");
        }
    }
}
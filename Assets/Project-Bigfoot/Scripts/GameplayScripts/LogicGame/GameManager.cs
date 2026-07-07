using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStatus currentStatus;

    public MusicDatabase musicDatabase;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "ProjectBigfoot")
        {
            ChangeStatus(GameStatus.EnCaceria);
        }
        else
        {
            ChangeStatus(GameStatus.MenuPrincipal);
        }
    }

    public void ChangeStatus(GameStatus newStatus)
    {
        currentStatus = newStatus;

        if (currentStatus == GameStatus.Pausa)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (currentStatus == GameStatus.MenuPrincipal)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (currentStatus == GameStatus.EnCaceria)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (currentStatus == GameStatus.Derrota || currentStatus == GameStatus.Victoria)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void StartGame()
    {
        ChangeStatus(GameStatus.EnCaceria);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeStatus(GameStatus.EnCaceria);
    }

    public void FinishGame()
    {
        ChangeStatus(GameStatus.Victoria);
    }

    public void BackToMenu()
    {
        ChangeStatus(GameStatus.MenuPrincipal);
        SceneManager.LoadScene("ProjectBigfoot");
    }
}
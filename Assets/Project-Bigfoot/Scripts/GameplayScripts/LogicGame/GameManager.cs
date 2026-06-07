using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStatus currentStatus;

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
        ChangeStatus(GameStatus.MenuPrincipal);
    }

    public void ChangeStatus(GameStatus newStatus)
    {
        currentStatus = newStatus;

        if (currentStatus == GameStatus.Pausa || currentStatus == GameStatus.MenuPrincipal)
        {
            Time.timeScale = 0f;
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
            Time.timeScale = 0f;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeStatus(GameStatus.EnCaceria);
    }

    public void FinishGame()
    {
        ChangeStatus(GameStatus.Victoria);
    }
}
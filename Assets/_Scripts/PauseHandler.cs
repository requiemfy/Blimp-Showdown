using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private void Awake()
    {
        pauseMenu.SetActive(false);
        GetComponent<Button>().onClick.AddListener(() => { 
            pauseMenu.SetActive(true);
            PauseGame();
        });
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        DataPersistence.Rematch();
        DOTween.KillAll();
        SceneManager.LoadScene(1);
        ResumeGame();
    }
    public void BackToMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);
        DataPersistence.ClearTeamDB();
        ResumeGame();
    }
}

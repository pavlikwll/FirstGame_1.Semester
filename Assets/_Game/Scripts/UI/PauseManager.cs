using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void Resume()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = 1f;
    }
}
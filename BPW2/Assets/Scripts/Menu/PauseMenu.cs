using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] 
    private GameObject PauseMenuUI;

    public static bool isPaused = false;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        FirstPersonController.cameraCanMove = true;
        PauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        FirstPersonController.cameraCanMove = false;
        PauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}

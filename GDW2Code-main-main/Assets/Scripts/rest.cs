using UnityEngine;
using UnityEngine.SceneManagement;

public class rest : MonoBehaviour
{
    public void RestTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public GameObject PauseMenu;
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }
}


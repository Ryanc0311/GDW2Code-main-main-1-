using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class win : MonoBehaviour
{
    public static bool Gamewin = false;

    public GameObject GamewinningUI;

    public void GameW()
    {
        Time.timeScale = 0f;
        GamewinningUI.SetActive(true);
        Gamewin = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

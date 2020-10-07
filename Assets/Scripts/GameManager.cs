using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float nextLevelTime = 0.1f;
    public StatusDisplay statusDisplay;

    private bool exitingLevel = false;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FailLevel()
    {
        if (exitingLevel)
            return;
        // Do lose animation/logic
        Invoke("RestartLevel", nextLevelTime);
        exitingLevel = true;
    }

    public void WinLevel()
    {
        if (exitingLevel)
            return;
        // Do win animation/logic
        Invoke("LoadNextLevel", nextLevelTime);
        exitingLevel = true;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

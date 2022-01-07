using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static bool gameIsPaused;
    public Button button;
    public GameObject pauseScreen;

    void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(TogglePause);
    }
    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1.0f;
        pauseScreen.SetActive(false);
    }

}
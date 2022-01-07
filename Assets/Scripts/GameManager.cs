using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        //Tarkistetaan, että on vain yksi instanssi. Jos löytyy jo olemassaoleva, se tuhotaan.
        if (_instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _instance = this;
        }
    }



    #endregion
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public Text nextLevelText;
    public GameObject nextLevel;

    public int AvailableLives;

    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }
    public static event Action<int> OnLiveLost;

    private void Start()
    {
        this.Lives = this.AvailableLives;
        //Pelin resoluutio ja full screen = true tai false.
        Screen.SetResolution(540, 960, false);
        Ball.onBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if(BricksManager.Instance.remainingBricks.Count <= 0)
        {
            StartCoroutine(NextLevelScreen());
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.loadNextLevel();
        }
    }

    public IEnumerator NextLevelScreen()
    {
        nextLevel.SetActive(true);
        yield return new WaitForSeconds(1.6f);
        nextLevel.SetActive(false);

    }/***/

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMainMenu()
    {
        PlayerPrefs.Save(); // kesken
        SceneManager.LoadScene("MainMenu");
        //GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
    }

    private void OnBallDeath(Ball obj)
    {
        //Jos palloja on jäljellä nolla, pelaaja häviää ja peli alkaa alusta.
        if (BallsManager.Instance.balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                //Näytetään game over ruutu.
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost?.Invoke(this.Lives);
                //Nollataan pallot ja ladataan taso uudelleen.
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.loadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    internal void ShowVictory()
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable()
    {
       Ball.onBallDeath -= OnBallDeath;
    }
}

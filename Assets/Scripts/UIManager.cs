using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TargetTxt;
    public Text ScoreTxt;
    public Text LivesTxt;

    public int score { get; set; }


    private void Awake()
    {
        //Invoket
        Brick.OnBrickDestruction += OnBrickDestruction;
        BricksManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLiveLost += OnLiveLost;
    }
    private void Start()
    {
        OnLiveLost(GameManager.Instance.AvailableLives);
    }

    private void OnLiveLost(int livesLeft)
    {
       LivesTxt.text = $@"Lives:
{livesLeft}";
    }

    private void OnLevelLoaded()
    {
        //P‰ivitet‰‰n tekstit.
       UpdateRemainingBricksText();
       UpdateScoreText(0);
    }

    private void UpdateScoreText(int i)
    {
        this.score += i;
        string scoreString = this.score.ToString().PadLeft(5, '0');
        ScoreTxt.text = $@"Score:
{scoreString}";
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10); //M‰‰rit‰ paljolla pisteet nousevat.
    }

    private void UpdateRemainingBricksText()
    {
        TargetTxt.text = $@"Target:
{BricksManager.Instance.remainingBricks.Count}/{BricksManager.Instance.initBricksCount}";
    }

    //Unsubscribaus
    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
    }
}

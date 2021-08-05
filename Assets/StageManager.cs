using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehavior<StageManager>
{
    public SaveInt highScore;
    public int score; // stageScore
    public SaveInt gold;

    new private void Awake()
    {
        base.Awake();
        highScore = new SaveInt("hightScore");
        gold = new SaveInt("gold");
        score = 0;
        ScoreUIRefresh();
        GoldUIRefresh();
    }
    public void AddScore(int addScore)
    {
        score += addScore;
        if (highScore.Value < score)
        {
            highScore.Value = score;
        }
        ScoreUIRefresh();
    }

    private void ScoreUIRefresh()
    {
        ScoreUI.Instance.UpdateUI(score, highScore.Value);
    }

    internal void AddGold(int amount)
    {
        gold += amount;
        GoldUIRefresh();
    }

    private void GoldUIRefresh()
    {
        GoldUI.Instance.UpdateUI(gold.Value);
    }
}

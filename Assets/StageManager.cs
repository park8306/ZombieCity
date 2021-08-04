using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehavior<StageManager>
{
    public SaveInt highScore;
    public int score; // stageScore

    new private void Awake()
    {
        base.Awake();
        highScore = new SaveInt("hightScore");
        score = 0;
    }
    public void AddScore(int addScore)
    {
        score += addScore;
        if (highScore.Value < score)
        {
            highScore.Value = score;
        }
        UIRefresh();
    }

    private void UIRefresh()
    {

    }
}

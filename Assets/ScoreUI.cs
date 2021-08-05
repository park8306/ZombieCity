using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : SingletonMonoBehavior<ScoreUI>
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highScoreText;

    protected override void OnInit()
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        highScoreText = transform.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
    }

    internal void UpdateUI(int score, int highScore)
    {
        scoreText.text = $"Score:{score.ToNumber()}";
        highScoreText.text = $"HighScore:{highScore.ToNumber()}";
    }
}

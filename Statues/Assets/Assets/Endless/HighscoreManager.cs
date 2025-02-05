using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighscoreManager
{
    private const string HighScoreKey = "HighScore";

    public static int HighScore
    {
        get => PlayerPrefs.GetInt(HighScoreKey, 0);
        private set => PlayerPrefs.SetInt(HighScoreKey, value);
    }

    public static void SaveHighScore(int currentScore)
    {
        if (currentScore > HighScore)
        {
            HighScore = currentScore;
            PlayerPrefs.Save();
        }
    }

    public static void ResetHighScore()
    {
        HighScore = 0;
        PlayerPrefs.Save();
    }
}
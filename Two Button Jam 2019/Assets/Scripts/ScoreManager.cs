﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score { get; private set; }
    public static int HighScore { get; private set; }
    public static int ScoreMultiplier { get; private set; }

    public static event Action ScoreChanged;

    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Start()
    {
        Score = 0;
        ScoreMultiplier = 1;
    }

    public static void AddPoints(int points)
    {
        Score += points * ScoreMultiplier;

        ScoreChanged?.Invoke();
    }

    public static void SetHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;

            PlayerPrefs.SetInt("HighScore", HighScore);
        }

        ScoreChanged?.Invoke();
    }

    public static void ResetScoreMultiplier()
    {
        ScoreMultiplier = 1;

        ScoreChanged?.Invoke();
    }

    public static void IncrementMultiplier()
    {
        ScoreMultiplier++;

        ScoreChanged?.Invoke();

    }
}

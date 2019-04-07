using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreMultiplier : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreMultiplierText = null;

    private void Awake()
    {
        ScoreMultiplierText.text = "x00";
    }

    private void Start()
    {
        SetScoreText();

        ScoreManager.ScoreChanged += ScoreManager_ScoreChanged;
    }

    private void ScoreManager_ScoreChanged()
    {
        SetScoreText();
    }

    private void OnDestroy()
    {
        ScoreManager.ScoreChanged -= ScoreManager_ScoreChanged;
    }

    private void SetScoreText()
    {
        ScoreMultiplierText.text = "× " + ScoreManager.ScoreMultiplier.ToString("00");
    }
}

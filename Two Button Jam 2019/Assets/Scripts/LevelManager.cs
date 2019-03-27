using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int CurrentLevelIndex = 0;

    private static LevelDef[] Levels;

    [SerializeField]
    private TMPro.TextMeshProUGUI LevelText = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI DescriptionText = null;

    private void Start()
    {
        if (Levels == null)
            Levels = Resources.LoadAll<LevelDef>("Levels").OrderBy(x => x.Number).ToArray();

        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Skip();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Skip()
    {
        Win();
    }

    private void LoadLevel()
    {
        LevelDef level = Levels[CurrentLevelIndex];

        LevelText.text = $"{level.Number}/{Levels.Length}";
        DescriptionText.text = level.DescriptionText;

        string layout = level.LevelLayout;

        int row = 0;

        foreach (var line in layout.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None))
        {
            int col = 0;

            foreach (var letter in line)
            {
                switch (letter)
                {
                    case 'W':
                        // Instantiate(WallPrefab, new Vector3(col, -row, 0), Quaternion.identity, transform);
                        break;

                    default:
                        break;
                }

                col++;
            }

            row++;
        }
    }

    public static void Win()
    {
        if (CurrentLevelIndex == Levels.Length - 1)
        {
            CurrentLevelIndex = 0;
            SceneManager.LoadScene("Win");
            return;
        }

        CurrentLevelIndex++;
        SceneManager.LoadScene("Main");
    }
}

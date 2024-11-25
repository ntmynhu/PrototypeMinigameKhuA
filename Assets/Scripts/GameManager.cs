using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public struct GameLevel
{
    public int level;
    public int paperNumber;
    public float totalTime;
    public float decreasingTime;
    public List<Color> colorList_L;
    public List<Color> colorList_R;
    public List<Color> colorList_Rejected;
}


public class GameManager : MonoBehaviour
{
    public List<Color> colorDatabase;
    public List<Color> colorDatabase_Rejected;

    private bool isPaused = false;

    #region Singleton

    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There is more than one instance of " + this);
            Destroy(this.gameObject);
        }
    }

    #endregion
    
    private GameLevel currentLevel;

    private void Start()
    {
        GameInit();
    }

    private GameLevel GetCurrentGameLevel()
    {
        return currentLevel;
    }

    private void GetColorBaseOnLevel(ref GameLevel gameLevel)
    {
        int randomIndex;

        if (gameLevel.level <= 3)
        {
            // Pick 1 random color for colorList_L
            gameLevel.colorList_L.Add(GetRandomColor(gameLevel));  

            // Pick 1 random color for colorList_R
            gameLevel.colorList_R.Add(GetRandomColor(gameLevel));
        }
        else if (gameLevel.level <= 8)
        {
            // Pick 2 random colors for colorList_L
            for (int i = 0; i < 2; i++)
            {
                gameLevel.colorList_L.Add(GetRandomColor(gameLevel));
            }

            // Pick 1 random color for colorList_R
            gameLevel.colorList_R.Add(GetRandomColor(gameLevel));
        } 
        else if (gameLevel.level <= 13)
        {
            // Pick 2 random colors for colorList_L
            for (int i = 0; i < 2; i++)
            {
                gameLevel.colorList_L.Add(GetRandomColor(gameLevel));
            }

            // Pick 2 random color for colorList_R
            for (int i = 0; i < 2; i++)
            {
                gameLevel.colorList_R.Add(GetRandomColor(gameLevel));
            }
        }
        else if (gameLevel.level >= 20)
        {
            // Pick 3 random colors for colorList_L
            for (int i = 0; i < 3; i++)
            {
                gameLevel.colorList_L.Add(GetRandomColor(gameLevel));
            }

            // Pick 3 random color for colorList_R
            for (int i = 0; i < 3; i++)
            {
                gameLevel.colorList_R.Add(GetRandomColor(gameLevel));
            }
        }
            
        // Pick random color for colorList_Rejected
        randomIndex = Random.Range(0, colorDatabase_Rejected.Count);
        gameLevel.colorList_Rejected.Add(colorDatabase_Rejected[randomIndex]);
    }

    private Color GetRandomColor(GameLevel gameLevel)
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, colorDatabase.Count);
        }
        while (gameLevel.colorList_L.Contains(colorDatabase[randomIndex]) || gameLevel.colorList_R.Contains(colorDatabase[randomIndex]));

        return colorDatabase[randomIndex];
    }

    public void GameInit()
    {
        currentLevel = new GameLevel()
        {
            level = 1,
            paperNumber = 20,
            totalTime = 33,
            decreasingTime = 3,
            colorList_L = new List<Color>(),
            colorList_R = new List<Color>(),
            colorList_Rejected = new List<Color>()
        };

        GetColorBaseOnLevel(ref currentLevel);

        GameUIManager.Instance.InitUI(currentLevel);
        PaperManager.Instance.PopulatePaper(currentLevel);
        GameUIManager.Instance.SetComputerText("\\(O_O)/\nAre you ready?");

        GameUIManager.Instance.StartCountDown(3f);
    }

    public void LevelUp()
    {
        GameUIManager.Instance.SetComputerText(TextDatabase.Instance.GetRandomLevelUpText());

        currentLevel.level++;

        if (currentLevel.level <= 3)
        {
            currentLevel.paperNumber += 5;
        }    
        else if (currentLevel.level <= 13)
        {
            currentLevel.paperNumber += 2;
        }    
            
        currentLevel.totalTime--;
        currentLevel.colorList_L.Clear();
        currentLevel.colorList_R.Clear();
        currentLevel.colorList_Rejected.Clear();

        GetColorBaseOnLevel(ref currentLevel);

        Debug.Log($"Level {currentLevel.level}; PaperNum {currentLevel.paperNumber}; Color Left {currentLevel.colorList_L.Count}; Color Right {currentLevel.colorList_R.Count}");

        GameUIManager.Instance.ResetUI();
        GameUIManager.Instance.InitUI(currentLevel);
        PaperManager.Instance.PopulatePaper(currentLevel);

        GameUIManager.Instance.StartCountDown(3f);
    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void PlayGame()
    {
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}

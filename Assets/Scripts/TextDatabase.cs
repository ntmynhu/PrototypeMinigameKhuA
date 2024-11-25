using System.Collections.Generic;
using UnityEngine;

public class TextDatabase : MonoBehaviour
{
    [SerializeField] private List<string> complimentList;
    [SerializeField] private List<string> comfortList;
    [SerializeField] private List<string> levelUpList;
    [SerializeField] private List<string> gameOverList;

    #region Singleton

    private static TextDatabase instance;
    public static TextDatabase Instance => instance;

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

    public string GetRandomCompilmentText()
    {
        return GetRandomText(complimentList);
    }

    public string GetRandomComfortText()
    {
        return GetRandomText(comfortList);
    }

    public string GetRandomLevelUpText()
    {
        return GetRandomText(levelUpList);
    }

    public string GetRandomGameOverText()
    {
        return GetRandomText(gameOverList);
    }

    private string GetRandomText(List<string> stringList)
    {
        if (stringList == null || stringList.Count == 0)
        {
            return string.Empty;
        }

        return stringList[Random.Range(0, stringList.Count)];
    }    
}

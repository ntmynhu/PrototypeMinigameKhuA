using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject leftNote;
    [SerializeField] private GameObject rightNote;
    [SerializeField] private GameObject rejectedNote;

    [SerializeField] private List<RectTransform> notePaperPosList;
    [SerializeField] private List<RectTransform> notePaperPosList_Rejected;

    [SerializeField] private Slider timer;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI computerText;

    [SerializeField] private GameObject wrongDirection;
    [SerializeField] private TextMeshProUGUI levelText;

    private float totalTime = 0;
    private float remainingTime = 0;

    #region Singleton

    private static GameUIManager instance;
    public static GameUIManager Instance => instance;

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

    public void InitUI(GameLevel gameLevel)
    {
        SpawnNote(gameLevel.colorList_L, notePaperPosList, leftNote);
        SpawnNote(gameLevel.colorList_R, notePaperPosList, rightNote);
        SpawnNote(gameLevel.colorList_Rejected, notePaperPosList_Rejected, rejectedNote);

        totalTime = gameLevel.totalTime;
        remainingTime = totalTime;

        HandleTimer();
        levelText.text = "Level " + gameLevel.level;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused())
        {
            HandleTimer();
        }
    }

    private void SpawnNote(List<Color> colorList, List<RectTransform> posList, GameObject notePrefab)
    {
        foreach (Color color in colorList)
        {
            foreach (var pos in posList)
            {
                if (!pos.gameObject.activeSelf)
                {
                    pos.gameObject.SetActive(true);
                    var spawnedObj = Instantiate(notePrefab, pos.transform);
                    Debug.Log("Instain");

                    Note note = spawnedObj.GetComponent<Note>();
                    note.SetColor(color);

                    break;
                }
            }
        }
    }

    private void HandleTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            float sliderValue = remainingTime / totalTime;
            timer.value = sliderValue;

            if (remainingTime < 0)
            {
                Debug.Log("Game Over");
                SetComputerText(TextDatabase.Instance.GetRandomGameOverText());
                Time.timeScale = 0;
                GameManager.Instance.PauseGame();
            }
        }
    }

    public void DecreasingTime(float value)
    {
        remainingTime -= value;
    }    

    public void ResetUI()
    {
        foreach (var pos in notePaperPosList)
        {
            if (pos.gameObject.activeSelf)
            {
                foreach (Transform child in pos.gameObject.transform)
                {
                    Destroy(child.gameObject);
                }

                pos.gameObject.SetActive(false);
            }
        }

        foreach (var pos in notePaperPosList_Rejected)
        {
            if (pos.gameObject.activeSelf)
            {
                foreach (Transform child in pos.gameObject.transform)
                {
                    Destroy(child.gameObject);
                }

                pos.gameObject.SetActive(false);
            }
        }
    }

    public void StartCountDown(float totalTime)
    {
        StartCoroutine(CountDown(totalTime));
    }

    private IEnumerator CountDown(float totalTime)
    {
        GameManager.Instance.PauseGame();
        countDownText.gameObject.SetActive(true);

        while (totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            countDownText.text = ((int)totalTime + 1).ToString();
            yield return null;
        }

        countDownText.text = "Go!";
        SetComputerText("\\(@>o<@)/\nGO!!");
        yield return new WaitForSeconds(.5f);

        countDownText.gameObject.SetActive(false);

        GameManager.Instance.PlayGame();
    }    

    public void SetComputerText(string text)
    {
        computerText.text = text;
    }    

    public void ShowWrongDirectionSign()
    {
        StartCoroutine(WrongDirectionSign());
    }    

    private IEnumerator WrongDirectionSign()
    {
        wrongDirection.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        wrongDirection.gameObject.SetActive(false);
    }    
}

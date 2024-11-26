using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    [SerializeField] private Paper paperPrefab;
    [SerializeField] private Transform initialPos;
    [SerializeField] private float moveDistance;
    [SerializeField] private float yAxisOffset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatPublisherSO wrongDirectionSO;

    private Stack<Paper> paperStack = new Stack<Paper>();
    private Vector3 currentPos = new Vector3();
    private Vector3 targetPos = new Vector3();

    private float decreasingTime = 0;

    #region Singleton

    private static PaperManager instance;
    public static PaperManager Instance => instance;

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

        currentPos = initialPos.position;
    }

    #endregion

    public void InitPaperStack(GameLevel gameLevel)
    {
        for (int i = 0; i < gameLevel.paperNumber; i++)
        {
            // Instantiate và setup giấy
            Paper paperSpawned = Instantiate(paperPrefab, currentPos, Quaternion.identity, this.transform);
            paperSpawned.SetUpPaper(gameLevel);
            paperStack.Push(paperSpawned);

            // Tăng sorting order và đặt random rotation
            paperSpawned.gameObject.GetComponent<SpriteRenderer>().sortingOrder = paperStack.Count;
            paperSpawned.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-1.5f, 1.5f));

            // Tăng vị trí cho tờ giấy tiếp theo
            currentPos.y += yAxisOffset;
        }

        decreasingTime = gameLevel.decreasingTime;
    }

    public void MovePaperToLeft()
    {
        if (paperStack.Count > 0)
        {
            targetPos = currentPos;
            targetPos.x -= moveDistance;

            StartCoroutine(MovePaper(targetPos, PaperType.Left));
        }
    }

    public void MovePaperToRight()
    {
        if (paperStack.Count > 0)
        {
            targetPos = currentPos;
            targetPos.x += moveDistance;

            StartCoroutine(MovePaper(targetPos, PaperType.Right));
        }
    }

    public void RejectPaper()
    {
        if (paperStack.Count > 0 && !GameManager.Instance.IsPaused())
        {
            Paper topPaper = paperStack.Pop();

            if (topPaper.type != PaperType.Rejected)
            {
                Debug.Log("Wrong");
                GameUIManager.Instance.SetComputerText(TextDatabase.Instance.GetRandomComfortText());
                GameUIManager.Instance.ShowWrongDirectionSign();
                //GameUIManager.Instance.DecreasingTime(decreasingTime);
                wrongDirectionSO.RaiseEvent(decreasingTime);
            }

            CheckAndDestroy(topPaper.gameObject);
        }
    }

    private IEnumerator MovePaper(Vector3 targetPos, PaperType paperType)
    {
        Paper topPaper = paperStack.Pop();

        while (Vector3.Distance(topPaper.transform.position, targetPos) > 0.01f)
        {
            topPaper.transform.position = Vector3.MoveTowards(topPaper.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Wrong if PaperType does not match the function that call Movepaper
        if (topPaper.type != paperType)
        {
            Debug.Log("Wrong");
            GameUIManager.Instance.SetComputerText(TextDatabase.Instance.GetRandomComfortText());
            GameUIManager.Instance.ShowWrongDirectionSign();
            GameUIManager.Instance.DecreasingTime(decreasingTime);

        }

        CheckAndDestroy(topPaper.gameObject);

        yield return null;
    }

    private void CheckAndDestroy(GameObject gameObject)
    {
        Destroy(gameObject);

        // Tỉ lệ khen là 1/5
        if (Random.Range(0, 5) == 0)
            GameUIManager.Instance.SetComputerText(TextDatabase.Instance.GetRandomCompilmentText());

        if (paperStack.Count == 0)
        {
            Debug.Log("Level Up");

            currentPos = initialPos.transform.position;
            GameManager.Instance.LevelUp();
        }    
    }
}

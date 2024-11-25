using UnityEngine;

public class PlayerPointer : MonoBehaviour
{
    [SerializeField] private float swipeOffset = .5f;
    [SerializeField] private VoidPublisherSO swipeLeftSo;
    [SerializeField] private VoidPublisherSO swipeRightSo;
    
    private Vector3 initialPos = new Vector3();
    private Vector3 currentPos = new Vector3();

    private float diffX = 0;

    private void Update()
    {
        if (!GameManager.Instance.IsPaused())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                initialPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialPos.z = 0;
            }

            currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            diffX = currentPos.x - initialPos.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(diffX) > swipeOffset)
            {
                if (diffX < 0)
                {
                    swipeLeftSo.RaiseEvent();
                }
                else
                {
                    swipeRightSo.RaiseEvent();
                }

                diffX = 0;
            }
        }
    }
}

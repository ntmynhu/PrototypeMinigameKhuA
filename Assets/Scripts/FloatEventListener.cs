using UnityEngine;
using UnityEngine.Events;

public class FloatEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> EventResponse;
    [SerializeField] private FloatPublisherSO publisher;

    private void OnEnable()
    {
        publisher.OnEventRaised += Response;
    }

    private void OnDisable()
    {
        publisher.OnEventRaised -= Response;
    }

    private void Response(float value)
    {
        EventResponse?.Invoke(value);
    }
}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Float Publisher", menuName = "ScriptableObjects/Events/Float Publisher")]
public class FloatPublisherSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float value)
    {
        OnEventRaised?.Invoke(value);
    }
}

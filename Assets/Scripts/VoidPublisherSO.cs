using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Void Publisher", menuName = "ScriptableObjects/Events/Void Publisher")]
public class VoidPublisherSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}

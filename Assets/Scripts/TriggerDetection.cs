using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
    }
}

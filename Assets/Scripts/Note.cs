using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private GameObject colorImage;

    public void SetColor(Color color)
    {
        Image image = colorImage.GetComponent<Image>();
        image.color = color;
    }
}

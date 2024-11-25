using UnityEngine;

public enum PaperType
{
    Left,
    Right,
    Rejected
}

public class Paper : MonoBehaviour
{
    public PaperType type;
    public SpriteRenderer spriteRenderer;

    public void SetUpPaper(GameLevel gamelevel)
    {
        int randomIndex = Random.Range(1, 6); // 1-2: Left | 3-4: Right | 5: Rejected
        int randomColor;

        if (randomIndex <= 2)
        {
            type = PaperType.Left;

            randomColor = Random.Range(0, gamelevel.colorList_L.Count);
            spriteRenderer.color = gamelevel.colorList_L[randomColor];
        }    
        else if (randomIndex <= 4)
        {
            type = PaperType.Right;

            randomColor = Random.Range(0, gamelevel.colorList_R.Count);
            spriteRenderer.color = gamelevel.colorList_R[randomColor];
        }   
        else
        {
            type = PaperType.Rejected;

            randomColor = Random.Range(0, gamelevel.colorList_Rejected.Count);
            spriteRenderer.color = gamelevel.colorList_Rejected[randomColor];
        }    
    }
}

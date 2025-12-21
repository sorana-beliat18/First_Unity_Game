using TMPro;
using UnityEngine;

public class StarUI : MonoBehaviour
{
    public TMP_Text starText;
    public int maxStars = 10;

    public void UpdateStars(int currentStars)
    {
        starText.text = currentStars + " / " + maxStars;
    }
}

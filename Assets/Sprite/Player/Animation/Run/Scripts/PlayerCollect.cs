using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int stars = 0;
    public int maxStars = 10;
    public StarUI starUI;

    public static int starsAtCheckpoint = 0;

    public Door door;

    void Start()
    {
        starUI.UpdateStars(stars);
    }

    public void AddStar()
    {
        stars++;
        starUI.UpdateStars(stars);

        if (stars >= maxStars)
        {
            Debug.Log("AI STRÂNS TOATE STELUȚELE!");
            door.OpenDoor(); 
        }
    }


    public void RestoreStarsFromCheckpoint()
{
    stars = starsAtCheckpoint;
    starUI.UpdateStars(stars);
}


    
}

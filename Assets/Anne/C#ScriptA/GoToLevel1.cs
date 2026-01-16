using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLevel1 : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_1");
    }
}

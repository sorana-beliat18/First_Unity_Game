using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenControl : MonoBehaviour
{
    public void StartGame()
    {
        
        SceneManager.LoadScene("Level_1");
    }

    public void QuitGame()
    {
        Debug.Log("Jocul s-a închis!");
        Application.Quit();
    }
}

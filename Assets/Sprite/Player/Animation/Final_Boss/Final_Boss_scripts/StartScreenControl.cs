using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenControl : MonoBehaviour
{
    // Această funcție va fi apelată de buton
    public void StartGame()
    {
        // "Nivel1" trebuie să fie numele EXACT al scenei tale de joc
        SceneManager.LoadScene("Level_1");
    }

    // Opțional: O funcție pentru a închide jocul
    public void QuitGame()
    {
        Debug.Log("Jocul s-a închis!");
        Application.Quit();
    }
}

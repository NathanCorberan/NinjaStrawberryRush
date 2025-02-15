using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        if(Player != null)
        {
            if (Player.transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f)
            {
                GameOver();
            }
        }
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);

    }
    public void ExitGame()
    {
        // Pour quitter l'application
        Application.Quit();

        // Si on est en mode éditeur, il ne quittera pas, donc on affiche un message dans la console.
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void GameOver()
    {
        // Vous pouvez charger une scène de game over ou afficher un écran de game over
        SceneManager.LoadScene("SceneGameOver");
    }
}

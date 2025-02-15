using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        if (Player.transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        // Vous pouvez charger une scène de game over ou afficher un écran de game over
        SceneManager.LoadScene("SceneGameOver");
    }
}

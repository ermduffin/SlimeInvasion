using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public void StartGame()
    {
        Enemy.gameOver = false;
        SceneManager.LoadScene("SampleScene");
    }
}

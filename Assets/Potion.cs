using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameController gameController;

    void OnTriggerEnter2D(Collider2D col)
    {
        // if picked up by the player
        if (col.gameObject.name == "Player")
        {
            // heal one life to the player
            gameController.PlayerHealed(gameObject);
        }
    }
}

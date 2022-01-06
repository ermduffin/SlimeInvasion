using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    public static int level;
    public static int totalKills;
    void Start()
    {
        if (gameObject.name == "LevelText")
        {
            // display the level reached by the player
            GetComponent<Text>().text += level;
        } else if (gameObject.name == "EnemyKillsText")
        {
            // display the total enemies killed by the player
            GetComponent<Text>().text += totalKills;
        }
        
    }
}

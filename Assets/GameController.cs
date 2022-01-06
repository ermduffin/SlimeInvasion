using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // grid list containing the four levels
    public List<GameObject> grids;
    // the tilemap covering the level's exit
    private GameObject tilemap;
    // the entrance to the next level
    public GameObject portal;
    // the cinemachine camera
    public GameObject vCamera;
    // the two camera bounds for small and large levels
    public List<GameObject> bounds;

    // the number of the current level and index of the level in grids
    private int level = 0;
    // difficulty modifier for enemy number
    private int difficulty = 1;
    // level size modifier for enemy number
    private int levelSize = 5;

    // time during which enemies can spawn
    private float levelTime;
    // text to display the countdown
    public Text levelTimeCountdown;

    // total of enemies killed across all levels
    private int totalKills = 0;
    // number of enemies to spawn on the current level
    private int enemyCount = 0;

    // the player's three lives
    public List<GameObject> lives;
    // the index of the current active life in the list
    private int curLife = 0;
    public Player player;
    private Spawner spawner;
    // the original potion object
    public GameObject potion;
    // the copy of the potion
    private GameObject tempPotion;

    void Awake()
    {
        calcLevelTime();
    }

    // calculate the time during which enemies can spawn for the current level
    private void calcLevelTime()
    {
        // levelTime starts at 10.99 and increments by 5 every other level
        levelTime = (level / 2 + 1) * 5 + 5.99f;
        // set the countdown text
        levelTimeCountdown.text = ((int)levelTime).ToString();
    }

    void Start()
    {
        // get the tilemap covering the exit
        tilemap = grids[level].transform.GetChild(0).gameObject;
        // get the enemy spawner
        spawner = grids[level].GetComponentInChildren<Spawner>();
        // start spawning enemies and save the number of enemies that will be spawned
        enemyCount = spawner.startEnemySpawn(levelSize, difficulty, levelTime, level);
    }

    void Update()
    {
        if (levelTime > 0)
        {
            // count down the leveltime
            levelTime -= Time.deltaTime;
            // update the countdown text
            levelTimeCountdown.text = ((int)levelTime).ToString();
        }

        if (enemyCount == 0)
        {
            // remove the tilemap covering the exit
            tilemap.SetActive(false);
            // create a potion
            tempPotion = Instantiate(potion, new Vector2(0,0), new Quaternion());
            enemyCount += 1;
        }
    }

    // called whenever an enemy dies
    public void EnemyKilled()
    {
        enemyCount--;
        totalKills++;
    }

    public void nextLevel()
    {
        // reactivate the tilemap covering the exit
        tilemap.SetActive(true);
        // deactivate the current level
        grids[level % grids.Count].SetActive(false);
        // increment to the next level
        level++;
        // activate the level
        grids[level % grids.Count].SetActive(true);
        // get the tilemap covering the exit
        tilemap = grids[level % grids.Count].transform.GetChild(0).gameObject;

        // if the current level is level 1 or 2
        if (level % grids.Count < 2)
        {
            // use the camera bounds for small levels
            vCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = bounds[0].GetComponent<PolygonCollider2D>();
            // set the entrance to the next level at the exit tunnel for small levels
            portal.transform.position = new Vector2(16,0.15f);
        // if the current level is level 3 or 4
        } else
        {
            // use the camera bounds for large levels
            vCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = bounds[1].GetComponent<PolygonCollider2D>();
            // set the entrance to the next level at the exit tunnel for large levels
            portal.transform.position = new Vector2(36,-2.75f);
        }
        
        // move the player to the start of the level
        player.gameObject.transform.position = player.startPosition;

        // destroy the temp potion if the player did not pick it up last level
        Destroy(tempPotion);

        // set the level size to 5 for small levels and 10 for large levels
        levelSize = ((level % grids.Count) / 2 + 1) * 5;
        difficulty++;
        calcLevelTime();
        // get the enemy spawner
        spawner = grids[level % grids.Count].GetComponentInChildren<Spawner>();
        // start spawning enemies and save the number of enemies that will be spawned
        enemyCount = spawner.startEnemySpawn(levelSize, difficulty, levelTime, level);
    }

    public void PlayerDamaged()
    {
        // deactivate the leftmost life
        lives[curLife].SetActive(false);
        // increment to the index of the next active life
        curLife++;
        // kill the player if all lives are deactivated
        if (curLife > 2)
            PlayerKilled();
    }

    public void PlayerHealed(GameObject potion)
    {
        // if the player has a deactivated life
        if (curLife - 1 >= 0 && lives[curLife - 1].activeInHierarchy == false)
        {
            // decrement to the deactivated life and then activate it
            lives[--curLife].SetActive(true);
            Destroy(potion);
        }
    }

    private void PlayerKilled()
    {
        Enemy.gameOver = true;
        // set the text in the game over scene based on the final level and score
        SetText.level = level + 1;
        SetText.totalKills = totalKills;
        SceneManager.LoadScene("GameOverScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject _enemy;
    public GameObject _fireEnemy;
    public GameObject player;
    public List<GameObject> spawnpoints;
    // the time during which an enemy can spawn
    private float _levelTime;

    // initiate enemy spawns for the level
    public int startEnemySpawn(int levelSize, int difficulty, float levelTime, int level)
    {
        // calculate the number of enemies to spawn
        int enemiesNum = levelSize + difficulty;

        _levelTime = levelTime;
        for (int i = 0; i < enemiesNum; i++)
        {
            StartCoroutine(SpawnEnemy(level));
        }

        return enemiesNum;
    }

    // spawn an enemy after a random amount of time within levelTime
    IEnumerator SpawnEnemy(int level)
    {
        // randomly choose a spawnpoint
        int spawnpoint = Random.Range(0, spawnpoints.Count);
        // randomly choose how long to wait until the enemy is spawned
        float spawnWaitTime = Random.Range(1f, _levelTime);

        yield return new WaitForSeconds(spawnWaitTime);

        // if the player is standing too close to the chosen spawnpoint
        if (player.transform.position.x - spawnpoints[spawnpoint].transform.position.x < 3 && player.transform.position.x - spawnpoints[spawnpoint].transform.position.x > -3)
        {
            // generate 0 or 1
            int rand = Random.Range(0, 2);
            // select the spawnpoint before or after the randomly selected spawnpoint in the spawnpoints list
            if (rand == 0)
            {
                spawnpoint -= 1;
            } else
            {
                spawnpoint += 1;
            }
            
            // if the spawnpoint selected is out of range of the list, loop back to the start or end of the list
            if (spawnpoint >= spawnpoints.Count)
            {
                spawnpoint = 0;
            } else if (spawnpoint < 0)
            {
                spawnpoint = spawnpoints.Count - 1;
            }
        }

        // randomly generate the type of enemy (0 = fire, 1,2 = default)
        int enemyType = Random.Range(0, 3);
        // if the player is on the 3rd level, spawn enemies of type 0 as fire slimes
        if (level > 1 && enemyType == 0)
        {
            Instantiate(_fireEnemy, spawnpoints[spawnpoint].transform.position, new Quaternion());
        } else
        {
            Instantiate(_enemy, spawnpoints[spawnpoint].transform.position, new Quaternion());
        }
    }
}

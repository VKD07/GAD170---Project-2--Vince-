using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] GameObject [] enemies;
    int randomNum;

    void Start()
    {
        //Starting the spawning loop of the enemy with random spawn delays
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        //creating an infinite loop that will spawn the enemies
        while (true)
        {
            //Going through all the elements of the array of enemies and instantiating them from the spawner position
            //with random delays
            for (int i = 0; i < enemies.Length; i++)
            {
                Instantiate(enemies[i], transform.position, Quaternion.identity);
                randomNum = UnityEngine.Random.Range(1, 6);
                yield return new WaitForSeconds(randomNum);
            }
        }
    }
}

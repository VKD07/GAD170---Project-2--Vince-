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
        StartCoroutine(SpawnEnemy());
    }

 
    IEnumerator SpawnEnemy()
    {
       
            for (int i = 0; i < enemies.Length; i++)
            {
                Instantiate(enemies[i], transform.position, Quaternion.identity);
                randomNum = UnityEngine.Random.Range(1, 6);
                yield return new WaitForSeconds(randomNum);
            }
        
    }
   
}

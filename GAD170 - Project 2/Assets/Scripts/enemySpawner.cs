using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] GameObject [] Enemies;

    int randomNum;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

 
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            for (int i = 0; i < Enemies.Length; i++)
            {
                Instantiate(Enemies[i], transform.position, Quaternion.identity);
                randomNum = UnityEngine.Random.Range(1, 6);
                yield return new WaitForSeconds(randomNum);
            }
        }
    }
   
}

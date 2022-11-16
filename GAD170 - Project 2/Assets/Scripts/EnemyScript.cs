using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{

    [Header("Enemy Stats")]
    [SerializeField] float speed = 2f;
    [SerializeField] int Health = 100;
    [SerializeField] float Damage = 5f;


    [Header("Score Reference")]
    ScoreHandler scoreHandler;
    [SerializeField] int pointsPerKill = 10;

    [Header("Enemy Droppable PowerUps")]
    [SerializeField] GameObject[] Powerups;

    //References
    GameObject Player;
    HealthBarRotation healthBar;



    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        scoreHandler = FindObjectOfType<ScoreHandler>();
        healthBar = GetComponentInChildren<HealthBarRotation>();

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        EnemyDeath();
        HealthBar();
    }

    private void HealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealthBar(Health);
        }
    }

    private void EnemyDeath()
    {
        if(Health <= 0)
        {
            scoreHandler.AddScore(pointsPerKill);
            DropAPowerUp();
            Destroy(this.gameObject);
        }
    }

    private void EnemyMovement()
    {
        float step = speed * Time.deltaTime;
        Vector3 enemyPos = new Vector3(transform.position.x, 0.68f, transform.position.z);
        transform.position = Vector3.MoveTowards(enemyPos, Player.transform.position, step);
    }

    void DropAPowerUp()
    {
       int randomNum = UnityEngine.Random.Range(0, Powerups.Length);

       Instantiate(Powerups[randomNum], transform.position, Quaternion.identity);
   
    }

    //Get and Set functions
    public void DamageEnemy(int Damage)
    {
        Health -= Damage;
    }

    public float EnemyDamage()
    {
        return Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Bomb")
        {
            Health = 0;
        }
    }
}

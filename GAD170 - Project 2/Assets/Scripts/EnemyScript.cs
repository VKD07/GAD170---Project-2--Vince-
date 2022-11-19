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
    [SerializeField] int health = 100;
    [SerializeField] float damage = 5f;


    [Header("Score Reference")]
    ScoreHandler scoreHandler;
    [SerializeField] int pointsPerKill = 10;

    [Header("Enemy Droppable PowerUps")]
    [SerializeField] GameObject[] powerUps;

    //References
   public GameObject player;
    HealthBarRotation healthBar;




    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
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
            healthBar.SetHealthBar(health);
        }
    }

    private void EnemyDeath()
    {
        if(health <= 0)
        {
            scoreHandler.AddScore(pointsPerKill);
            DropAPowerUp();
            Destroy(this.gameObject);
        } 
    }



    private void EnemyMovement()
    {
        float step = speed * Time.deltaTime;
        Vector3 enemyPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(enemyPos, player.transform.position, step);

        transform.LookAt(player.transform);

    }

    void DropAPowerUp()
    {
       int randomNum = UnityEngine.Random.Range(0, powerUps.Length);

       Instantiate(powerUps[randomNum], transform.position, Quaternion.identity);
   
    }

    //Get and Set functions
    public void DamageEnemy(int damage)
    {
        health -= damage;
    }

    public float EnemyDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Bomb")
        {
            health = 0;
        }
    }
}

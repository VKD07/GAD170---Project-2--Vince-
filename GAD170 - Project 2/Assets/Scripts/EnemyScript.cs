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

    [Header("Particle System")]
    [SerializeField] ParticleSystem explosion;

    [Header("Sounds")]
    [SerializeField] AudioClip explosionSound;
    AudioSource audioSource;
   
     //other references
     GameObject player;
     HealthBarRotation healthBar;

    void Start()
    {
        //finding the game object that has tag player
        player = GameObject.FindGameObjectWithTag("Player");

        //find the object that has score handler script
        scoreHandler = FindObjectOfType<ScoreHandler>();

        //Getting the health bar rotation scrip from the children of this object
        healthBar = GetComponentInChildren<HealthBarRotation>();

        //Finding a game object that has tag audio player and getting its audio source component
        audioSource = GameObject.FindGameObjectWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Responsible for the enemy movement
        EnemyMovement();

        //Responsible for the enemy death
        EnemyDeath();

        //Responsible for the health bar UI
        HealthBar();
    }

    private void HealthBar()
    {
        //If the health bar script is not there then set the health bar to this enemies health
        if (healthBar != null)
        {
            healthBar.SetHealthBar(health);
        }
    }

    private void EnemyDeath()
    {
        //If the enemy reaches the health of 0, then play an explosion sound
        //Add the score to the UI
        //Drop a power up
        //Destroy this enemy
        if(health <= 0)
        {
            explosion.Play();
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(explosionSound);
            }
           
            scoreHandler.AddScore(pointsPerKill);
            DropAPowerUp();
            Destroy(this.gameObject, 0.1f);
        } 
    }

    private void EnemyMovement()
    {
        //Enemy is always going to the players position with a certain speed
        float step = speed * Time.deltaTime;
        Vector3 enemyPos = transform.position;
        transform.position = Vector3.MoveTowards(enemyPos, player.transform.position, step);

        //Always look at the player
        transform.LookAt(player.transform);
    }

    void DropAPowerUp()
    {
        //creating an integer variable that store random numbers from 0 to the number of power ups available in the array
        int randomNum = UnityEngine.Random.Range(0, powerUps.Length);

        //Instatiating random power ups from this enemy's position without rotation
        Instantiate(powerUps[randomNum], transform.position, Quaternion.identity);
    }

    //Below are the getter and setter functions for cross referencing ---------------------------------------------------------------

    //Used for receiving damage
    public void DamageEnemy(int damage)
    {
        health -= damage;
    }

    //Used to provide the enemy damage from this script
    public float EnemyDamage()
    {
        return damage;
    }

    //Collision Handler---------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        //If this enemy is colliding a bomb then kill this enemy immediately
       if(other.tag == "Bomb")
        {
            health = 0;
        }
    }
}

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] Transform playerCamera;
    [SerializeField] float normalSpeed = 3.5f;
    [SerializeField] float walkSpeed = 3.5f;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float mouseSensitivityX = 100f;
    [SerializeField] float mouseSensitivityY = 100f;
    bool isGrounded = true;
    float xRotation;

    [Header("Player Gun")]
    [SerializeField] float damage = 25f;
    [SerializeField] Transform gunPosition;
    [SerializeField] float bulletMaxDistance = 100f;
    [SerializeField] LayerMask bulletTarget;
    [SerializeField] Animator gunAnimation;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Sounds")]
    [SerializeField] AudioClip GunShot;
    [SerializeField] AudioSource audioSource;

    [Header("Player Stats")]
    [SerializeField] int health = 100;

    [Header("PowerUps")]
    [SerializeField] float shieldDuration = 5f;
    bool shield = false;

    [Header("DeathScreen Reference")]
    [SerializeField] GameObject deathScreen;

    //Player Components
    Rigidbody playerRB;

    //Event Delegate
    public delegate void DeathEvent();
    public DeathEvent deathEvent;

    void Start()
    {
        //The cursor will be locked in the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        //Getting the player Rigid Body Component
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Responsible for the player movement
        PlayerMovement(); 
        //Responsible for the Gun
        PlayerGun();
        //Responsible if ever the player collected a shield
        ShieldItem();
        //Responsible if the player dies
        DeathHandler();
    }

    private void DeathHandler()
    {
        //If the health is 0 or less then the death screen UI will be activate, triggering the the functions that are subscribed to the death event.
       if(health <= 0)
        {
                deathScreen.SetActive(true);
                deathEvent();
        }
        else
        {
            //If the health is not 0 then the game is unpaused;
            Time.timeScale = 1f;
        }
    }

    private void PlayerMovement()
    {
        //Getting Keyboard Input Values
        float zPos = Input.GetAxis("Vertical");
        float xPos = Input.GetAxis("Horizontal");
        //Getting the mouse axis and multiplying it with a float, so that we can control the mouse sensitivty.
        float xMouse = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;
        
        // Player Movement multiplying it with float speed so that we can control the speed value.
        Vector3 playerPos = transform.forward * zPos + transform.right * xPos;
        transform.position += playerPos * normalSpeed * Time.deltaTime;

        //Player sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            normalSpeed = runSpeed;
        }
        else
        {
            normalSpeed = walkSpeed;
        }

        //Mouse Rotation for the camera
        transform.Rotate(0f, xMouse, 0f);
        xRotation -= yMouse;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);
        playerCamera.transform.eulerAngles = new Vector3(xRotation, playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);
   
        //The player can press space and jump if it is touching the ground
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded == true)
            {
                isGrounded = false;
                Vector3 playerJumpPos = new Vector3(0f, jumpForce, 0f);
                playerRB.AddForce(playerJumpPos);
            }
        }
    }

    public void PlayerGun()
    {
        //Creating a ray, from gun position to the forward direction of the gun position
        Ray ray = new Ray(gunPosition.position, gunPosition.TransformDirection(Vector3.forward));
        //Ray cast hit to store the values of whatever object is hit by the ray
        RaycastHit hit;
        
        //if the ray hits the player, then player can begin to shoot, this will trigger the sound, the particles, the animation and damages the enemy
        if(Physics.Raycast(ray, out hit, bulletMaxDistance, bulletTarget))
        {
            //creating a line to see the ray, The color will turn red if it hits an enemy
            Debug.DrawLine(gunPosition.position, hit.point, Color.red);

            if(hit.collider.gameObject.tag == "Enemy" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                audioSource.PlayOneShot(GunShot);
                gunAnimation.SetTrigger("FireGun");
                hit.collider.gameObject.GetComponent<EnemyScript>().DamageEnemy((int)damage);
                muzzleFlash.Play();
            }
        }
        else
        {
            //the line ray will be green if its not hitting the enemy
            Debug.DrawLine(gunPosition.position, gunPosition.TransformDirection(Vector3.forward) * bulletMaxDistance, Color.green);
        }
    }

    private void ShieldItem()
    {
        //If the shield has been collided then print it to the consol and start the shield duration
        if(shield == true)
        {
            print("Shield has now been Activated");
            StartCoroutine(StartShieldTimer());
        }
    }

    //Shield timer
    IEnumerator StartShieldTimer()
    {
        yield return new WaitForSeconds(shieldDuration);
        print("Shield is now deactivated");
        shield = false;
    }

    //Below are the getter and settter functions that is used to cross reference--------------------------------

    //Returning the player health value
    public int PlayerHealth()
    {
        return health;
    }

    //Setting the player health
    public void AddPlayerHealth(int health)
    {
        this.health += health;
        
        if(this.health > 100)
        {
            this.health = 100;
        }
    }

    //Returning the damage of the player
    public float GetPlayerDamage()
    {
        return damage;
    }

    //Collision Handler -----------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        //if the enemy collides with this player and shield isnt activated then, the player health will be deducted and the enemy will be destroyed
        if(collision.gameObject.tag == "Enemy")
        {
            if (shield == false)
            {
                health -= (int)collision.gameObject.GetComponent<EnemyScript>().EnemyDamage();
                Destroy(collision.gameObject);
            }
        }

        //If the player is touching the ground then tell the system that player is in the ground
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
  
    private void OnTriggerEnter(Collider other)
    {
        //if the player collided with an object that has tag shield, then tell the system to activate the shield and destroy the shield power up
        if (other.tag == "Shield")
        {
            shield = true;
            Destroy(other.gameObject);
        }
    }

}// End of Class

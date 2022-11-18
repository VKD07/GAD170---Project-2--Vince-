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
    [SerializeField] float NormalSpeed = 3.5f;
    [SerializeField] float WalkSpeed = 3.5f;
    [SerializeField] float RunSpeed = 7f;
    [SerializeField] float JumpForce = 10f;
    [SerializeField] float MouseSensitivityX = 100f;
    [SerializeField] float MouseSensitivityY = 100f;
    bool isGrounded = true;
    float xRotation;

    [Header("Player Gun")]
    [SerializeField] float Damage = 25f;
    [SerializeField] Transform GunPosition;
    [SerializeField] float BulletMaxDistance = 100f;
    [SerializeField] LayerMask bulletTarget;
    [SerializeField] Animator GunAnimation;

    [Header("Player Stats")]
    [SerializeField] int Health = 100;

    [Header("PowerUps")]
    [SerializeField] GameObject bombItem;
    [SerializeField] float ShieldDuration = 5f;
    bool Shield = false;

    [Header("Death Reference")]
    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject HealthUI;
    [SerializeField] GameObject ScoreUI;
    [SerializeField] GameObject GunSightUI;

    //Player Components
    Rigidbody playerRB;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRB = GetComponent<Rigidbody>();
        DeathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerGun();
        BombItem();
        ShieldItem();
        DeathHandler();

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Break();
        }
    }

    private void DeathHandler()
    {
       if(Health <= 0)
        {
            DeathScreen.SetActive(true);
         
            HealthUI.SetActive(false);
            ScoreUI.SetActive(false);
            GunSightUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            this.GetComponent<PlayerScript>().enabled = false;
        }
    }

    private void PlayerMovement()
    {
        //Getting Keyboard Input Values
        float zPos = Input.GetAxis("Vertical");
        float xPos = Input.GetAxis("Horizontal");
        float xMouse = Input.GetAxis("Mouse X") * MouseSensitivityX * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") *MouseSensitivityY * Time.deltaTime;
        
        // Player Movement
        Vector3 playerPos = transform.forward * zPos + transform.right * xPos;
        transform.position += playerPos * NormalSpeed * Time.deltaTime;

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            NormalSpeed = RunSpeed;
        }
        else
        {
            NormalSpeed = WalkSpeed;
        }

        //Mouse Rotation
        transform.Rotate(0f, xMouse, 0f);

        xRotation -= yMouse;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);
        playerCamera.transform.eulerAngles = new Vector3(xRotation, playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);
   
        //Player Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded == true)
            {
                Vector3 playerJumpPos = new Vector3(0f, JumpForce, 0f);
                playerRB.AddForce(playerJumpPos);
            }
        }
    }

    public void PlayerGun()
    {
        Ray ray = new Ray(GunPosition.position, GunPosition.TransformDirection(Vector3.forward));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, BulletMaxDistance, bulletTarget))
        {
            Debug.DrawLine(GunPosition.position, hit.point, Color.red);

            if(hit.collider.gameObject.tag == "Enemy" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                GunAnimation.SetTrigger("FireGun");

                hit.collider.gameObject.GetComponent<EnemyScript>().DamageEnemy((int)Damage);
            }
        }
        else
        {
            Debug.DrawLine(GunPosition.position, GunPosition.TransformDirection(Vector3.forward) * BulletMaxDistance, Color.green);
        }
    }

    private void BombItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(bombItem, transform.position, Quaternion.identity);
        }
    }

    private void ShieldItem()
    {
        if(Shield == true)
        {
            print("Shield has now been Activated");
            StartCoroutine(StartShieldTimer());
        }
    }

    //Shield timer
    IEnumerator StartShieldTimer()
    {
        yield return new WaitForSeconds(ShieldDuration);
        print("Shield is now deactivated");
        Shield = false;
    }

    //Get and Set Functions
    public int PlayerHealth()
    {
        return Health;
    }

    public void AddPlayerHealth(int health)
    {
        Health += health;
        
        if(Health > 100)
        {
            Health = 100;
        }
    }

    public float GetPlayerDamage()
    {
        return Damage;
    }

    //Collision Handle
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (Shield == false)
            {
                Health -= (int)collision.gameObject.GetComponent<EnemyScript>().EnemyDamage();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
            Shield = true;
            Destroy(other.gameObject);
        }
    }

}// End of Class

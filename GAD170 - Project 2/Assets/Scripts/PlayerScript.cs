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

    //Delagate
    public delegate void DeathEvent();
    public DeathEvent deathEvent;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerGun();
        ShieldItem();
        DeathHandler();

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Break();
        }
     
    }

    private void DeathHandler()
    {
       if(health <= 0)
        {
                deathScreen.SetActive(true);
                deathEvent();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void PlayerMovement()
    {
        //Getting Keyboard Input Values
        float zPos = Input.GetAxis("Vertical");
        float xPos = Input.GetAxis("Horizontal");
        float xMouse = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;
        
        // Player Movement
        Vector3 playerPos = transform.forward * zPos + transform.right * xPos;
        transform.position += playerPos * normalSpeed * Time.deltaTime;

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            normalSpeed = runSpeed;
        }
        else
        {
            normalSpeed = walkSpeed;
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
                isGrounded = false;
                Vector3 playerJumpPos = new Vector3(0f, jumpForce, 0f);
                playerRB.AddForce(playerJumpPos);
            }
        }
    }

    public void PlayerGun()
    {
        Ray ray = new Ray(gunPosition.position, gunPosition.TransformDirection(Vector3.forward));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, bulletMaxDistance, bulletTarget))
        {
            Debug.DrawLine(gunPosition.position, hit.point, Color.red);

            if(hit.collider.gameObject.tag == "Enemy" && Input.GetKeyDown(KeyCode.Mouse0))
            {
                gunAnimation.SetTrigger("FireGun");
                hit.collider.gameObject.GetComponent<EnemyScript>().DamageEnemy((int)damage);
                muzzleFlash.Play();
            }
        }
        else
        {
            Debug.DrawLine(gunPosition.position, gunPosition.TransformDirection(Vector3.forward) * bulletMaxDistance, Color.green);
        }
    }

    private void ShieldItem()
    {
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

    //Get and Set Functions
    public int PlayerHealth()
    {
        return health;
    }

    public void AddPlayerHealth(int health)
    {
        this.health += health;
        
        if(this.health > 100)
        {
            this.health = 100;
        }
    }

    public float GetPlayerDamage()
    {
        return damage;
    }

    //Collision Handle
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (shield == false)
            {
                health -= (int)collision.gameObject.GetComponent<EnemyScript>().EnemyDamage();
                Destroy(collision.gameObject);
            }
        }

        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
            shield = true;
            Destroy(other.gameObject);
        }
    }

}// End of Class

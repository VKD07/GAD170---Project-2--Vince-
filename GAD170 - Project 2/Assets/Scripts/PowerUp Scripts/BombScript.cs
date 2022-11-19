using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float timeToExplode = 2f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioSource audioSource;
    SphereCollider sphereCollider;
    
    void Start()
    {
        //Getting the sphere collider of this object
        sphereCollider = GetComponent<SphereCollider>();
        
        //Disabling first the sphere collider
        sphereCollider.enabled = false;
    }

    void Update()
    {
        // Start the bomb countdown before it explodes
        StartCoroutine(BombTimer());
    }

    IEnumerator BombTimer()
    {
        //Setting the delay time of the explosion
        yield return new WaitForSeconds(timeToExplode);

        //The sound of the explosion once it explodes
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(explosionSound);
        }
      
        //Playing the explosion particle effect
        explosion.Play();

        //Enabling the sphere collider
        sphereCollider.enabled = true;

        //then destroying this object after 0.5 seconds
        Destroy(this.gameObject, 0.5f);
    }
}

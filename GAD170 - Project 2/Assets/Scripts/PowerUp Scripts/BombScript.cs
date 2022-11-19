using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] ParticleSystem Explosion;
    [SerializeField] float TimeToExplode = 2f;
    SphereCollider sphereCollider;
    

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(BombTimer());

    }


    IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(TimeToExplode);
        Explosion.Play();
        sphereCollider.enabled = true;
        Destroy(this.gameObject, 0.1f);
    }

}

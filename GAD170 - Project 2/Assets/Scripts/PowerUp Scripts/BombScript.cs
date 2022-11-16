using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
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

        sphereCollider.enabled = true;
        Destroy(this.gameObject, 0.1f);
    }

}

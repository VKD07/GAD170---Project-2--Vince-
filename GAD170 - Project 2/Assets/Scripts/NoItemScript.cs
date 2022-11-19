using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoItemScript : MonoBehaviour
{
   //If this blank Item spawns then it will be destroyed immediately after 1 second.
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}

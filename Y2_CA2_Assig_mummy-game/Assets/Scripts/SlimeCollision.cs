using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCollision : MonoBehaviour
{
    public SlimeJelly SlimeJelly;
    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Player")
            SlimeJelly.Icalled = true;
    }
}

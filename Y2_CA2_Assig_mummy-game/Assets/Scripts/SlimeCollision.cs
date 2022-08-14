using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to slime collision events
//---------------------------------------------------------------------------------
public class SlimeCollision : MonoBehaviour
{
    [Header("Slime Script")]
    public SlimeJelly SlimeJelly;
    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Player")
            SlimeJelly.Icalled = true;
    }
}

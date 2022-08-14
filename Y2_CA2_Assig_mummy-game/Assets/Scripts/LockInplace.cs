using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle the player hitbox.
//                This script ensures that the player hitbox remains above ground.
//---------------------------------------------------------------------------------
public class LockInplace : MonoBehaviour
{
    [Header("Player Locations")]
    public Transform Camera;
    public Transform Hitbox;

    // How high above the ground do I want it to be.
    public float idealHight;

    void Update()
    {
        // if hitbox is below and above ideal height, is move X and Z position if scroll wheel is used to move.
        if (Hitbox.transform.position.y < -2.7 || Hitbox.transform.position.y > -2.8 || Hitbox.transform.position.x != Camera.position.x || Hitbox.transform.position.z != Camera.position.z)
        {
            Hitbox.transform.position = new Vector3(Camera.position.x, idealHight, Camera.position.z);
        }
    }
}

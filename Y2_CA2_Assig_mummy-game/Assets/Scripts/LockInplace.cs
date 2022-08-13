using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInplace : MonoBehaviour
{
    public Transform Camera;
    public Transform Hitbox;
    public float idealHight;

    void Update()
    {
        if (Hitbox.transform.position.y < -2.7 || Hitbox.transform.position.y > -2.8 || Hitbox.transform.position.x != Camera.position.x || Hitbox.transform.position.z != Camera.position.z)
        {
            Hitbox.transform.position = new Vector3(Camera.position.x, idealHight, Camera.position.z);
        }
    }
}

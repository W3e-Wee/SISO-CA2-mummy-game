using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour
{
    public LeverControl lever1;
    public LeverControl lever2;
    public LeverControl lever3;
    public LeverControl lever4;

    public bool isOpen = false;

    private void FixedUpdate()
    {
        if (lever1.isActivated && lever2.isActivated && lever3.isActivated && lever4.isActivated)
        {
            Debug.Log("Door Open!");
        }
    }
}

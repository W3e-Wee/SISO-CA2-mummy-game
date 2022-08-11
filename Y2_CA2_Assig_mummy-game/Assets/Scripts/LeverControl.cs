using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverControl : MonoBehaviour
{
    public Transform activatePoint;
    public XRGrabInteractable lever;
    public Transform leaver;
    public float threshold = 0.06f;
    public bool isActivated = false;

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(activatePoint.position, leaver.position);
        // Debug.Log(!isActivated + " " + distance);

        if (distance < threshold && !isActivated)
        {
            isActivated = true;
            lever.enabled = false;
        }
    }

}

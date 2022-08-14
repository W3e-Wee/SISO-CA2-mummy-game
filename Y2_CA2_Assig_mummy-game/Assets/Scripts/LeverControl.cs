using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle Lever events
//---------------------------------------------------------------------------------
public class LeverControl : MonoBehaviour
{
    [Header("Lever Settings")]
    // this is the trigger that is used to see if the lever is pulled or not
    public Transform activatePoint;

    // this is to get the lever's grab interactable to disable later
    public XRGrabInteractable lever;
    // this gets the location of the leavers
    public Transform leverLocation;

    // how close the lever needs to be to the lever to be activated
    public float threshold = 0.06f;

    // this is the bool that is activated when the lever is activated
    public bool isActivated = false;

    private void Start() {
        leverLocation = this.transform.GetChild(0);
        lever = leverLocation.GetComponent<XRGrabInteractable>();
        activatePoint = this.transform.GetChild(1);
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(activatePoint.position, leverLocation.position);
        // Debug.Log(!isActivated + " " + distance);
        if (distance < threshold && !isActivated)
        {
            isActivated = true;
            lever.enabled = false;
        }
    }

}

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//---------------------------------------------------------------------------------
// Youtube      : https://www.youtube.com/watch?v=UlqdHrfXppo
// Description	: This will check which HMD is available. If not try to use MockHMD.
//---------------------------------------------------------------------------------
public class HMDInfoManager : MonoBehaviour 
{
    //===================
    // Private Variables
    //===================
    [SerializeField] GameObject mockSimulator;
    [SerializeField] GameObject XRrigcam;
	
    //---------------------------------------------------------------------------------
    // Start is when Script is active
    //---------------------------------------------------------------------------------
    protected void Start() 
    {
        Debug.Log("Is Device Active: " + XRSettings.isDeviceActive);
        Debug.Log("Device Name is : " + XRSettings.loadedDeviceName);

        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset plugged in");
            mockSimulator.SetActive(true);
        }
        else if (XRSettings.isDeviceActive &&  XRSettings.loadedDeviceName == "MockHMD Display")
        {
            Debug.Log("Using Mock HMD");
            mockSimulator.SetActive(true);
            //2022-05-31
            //XR Interaction Toolkit 1.x
            //XRrigcam.GetComponent<TrackedPoseDriver>().rotationAction.ApplyBindingOverride("<XRSimulatedHMD>/centerEyeRotation");
            //XR Interaction Toolkit 2.x
            InputAction rotateHeadAction = XRrigcam.GetComponent<TrackedPoseDriver>().rotationAction;

            InputBinding inputBinding = rotateHeadAction.bindings[0];
            inputBinding.overridePath = "<XRSimulatedHMD>/centerEyeRotation";
            rotateHeadAction.ApplyBindingOverride(0, inputBinding);        
        }
        else
        {
            Debug.Log("We Have a Headset " + XRSettings.loadedDeviceName);
            mockSimulator.SetActive(false);
            XRrigcam.GetComponent<TrackedPoseDriver>().rotationAction.RemoveAllBindingOverrides();
        }
        // Lock Mouse Cursor at center of Game Window and hide it. Press ESC to see cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
	
}

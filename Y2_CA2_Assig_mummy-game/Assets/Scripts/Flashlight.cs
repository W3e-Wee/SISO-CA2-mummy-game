using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//---------------------------------------------------------------------------------
// Author		: Wee Heng
// Date  		: 2022-08-09
// Modified By	: Wee Heng
// Modified Date: 2022-08-09
// Description	: Script to handle flashlight events
//---------------------------------------------------------------------------------

public class Flashlight : MonoBehaviour
{
    #region Public Variables
    public Light _spotLightSource;

    [Header("RayCasy Settings")]
    public int _range;

    public Camera vrCam;
    #endregion

    void Update()
    {
        
    }

    #region Public Methods
    public void LightKillRay() // changes light color & angle along with some other things
    {
        _spotLightSource.spotAngle = 45;
        _spotLightSource.color = Color.cyan;
        _spotLightSource.intensity = 2;
        // Calls a function
        Kill();
    }

    public void LightDefault() // returns light angle and color to default state
    {
        _spotLightSource.spotAngle = 60;
        _spotLightSource.color = Color.white;
        _spotLightSource.intensity = 1;
    }
    #endregion

    #region Private Variables
    private void Kill() // Raycast to get enemy info 
    {
        // Create a raycast
        RaycastHit hit;

        if (Physics.Raycast(vrCam.transform.position, vrCam.transform.forward, out hit, _range)) // Send raycast forward from position of vrCam
        {
            // if raycast hit something
            Debug.Log(hit.transform.name); // Get name of collided target
        }
    }
    #endregion
}

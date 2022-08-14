using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

//---------------------------------------------------------------------------------
// Author		: Wee Heng, Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle flashlight events
//---------------------------------------------------------------------------------

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    #region Public Variables
    public Light _spotLightSource;
    public bool _killMode = false;

    [Header("RayCasy Settings")]
    public int _range;
    public Camera Cam;

    #endregion



    void Update() {
        // This is a if else to see if kill mode is on or off
        
        if (_killMode == true) {
            // if kill mode is true change flashlight color
            _spotLightSource.spotAngle = 45;
            _spotLightSource.color = Color.cyan;
            _spotLightSource.intensity = 2;

            // Create a raycast
            RaycastHit hit; 
            if (Physics.Raycast(Cam.transform.position,Cam.transform.forward, out hit, _range)) // Send raycast forward from position of vrCam
            {
                // if raycast hit something
                // Debug.Log(hit.transform.tag); // Get name of collided target

                // if collided target is slime
                if (hit.transform.tag == "slime") {
                     
                    // if health is higher than 0, Hurt the slime by 1 HP
                    if (hit.transform.GetComponent<SlimeJelly>().health > 0) {
                        hit.transform.GetComponent<SlimeJelly>().health -= 1;
                    }
                }

                // if collided target is mummy
                if (hit.transform.tag == "mummy") {
                    hit.transform.GetComponent<mummyPathing>()._state = mummyPathing.STATE.stunned;
                }
            }
        }
        else {
            // if kill mode is false change flashlight color
            _spotLightSource.spotAngle = 60;
            _spotLightSource.color = Color.white;
            _spotLightSource.intensity = 1;
        }
    }

    // this is put in the Grab Interactable of the flashlight
    // it is activated when trigger is pressed, this enables killmode.
    public void Activate() {
        _killMode = true;
    }

    // this is put in the Grab Interactable of the flashlight
    // it is activated when trigger is released, this disables killmode.
    public void Deactivate() {
        _killMode = false;
    }
}

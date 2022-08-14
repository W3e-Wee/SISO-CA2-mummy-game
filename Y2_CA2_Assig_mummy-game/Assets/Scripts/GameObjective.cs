using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle flashlight events
//---------------------------------------------------------------------------------
public class GameObjective : MonoBehaviour
{
    [Header("Levers")]
    public LeverControl lever1;
    public LeverControl lever2;
    public LeverControl lever3;
    public LeverControl lever4;

    [Header("Animation")]
    public Animator exit1_ani;
    public Animator exit2_ani; 

    [Header("Buttons")]
    public Button exitbutton1;
    public Button exitbutton2;

    [Header("Final Objective")]
    public bool isOpen = false;

    private void FixedUpdate()
    {
        // This checks to see if all levers are flicked open
        if (lever1.isActivated && lever2.isActivated && lever3.isActivated && lever4.isActivated)
        {
            isOpen = true;
        }

        // if all leavers are flicked open up the doors
        if (isOpen)
        {
            exit1_ani.SetBool("isOpen", true);
            exit2_ani.SetBool("isOpen", true);
        }
    }

    // This occurs when the exit button is pressed at the exit gates
    // It sends the users back to the main menu.
    public void exit()
    {
        if (isOpen) {
            SceneManager.LoadScene("menu");
        }
    }
}

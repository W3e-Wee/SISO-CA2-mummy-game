using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameObjective : MonoBehaviour
{
    public LeverControl lever1;
    public LeverControl lever2;
    public LeverControl lever3;
    public LeverControl lever4;

    public Animator exit1_ani;
    public Animator exit2_ani; 

    public Button exitbutton1;
    public Button exitbutton2;

    public bool isOpen = false;

    private void FixedUpdate()
    {
        if (lever1.isActivated && lever2.isActivated && lever3.isActivated && lever4.isActivated)
        {
            isOpen = true;
        }

        if (isOpen)
        {
            exit1_ani.SetBool("isOpen", true);
            exit2_ani.SetBool("isOpen", true);
        }
    }

    public void exit()
    {
        if (isOpen) {
            SceneManager.LoadScene("menu");
        }
    }
}

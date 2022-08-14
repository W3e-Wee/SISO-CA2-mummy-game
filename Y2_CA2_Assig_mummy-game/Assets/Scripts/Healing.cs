using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle healing events
//---------------------------------------------------------------------------------
public class Healing : MonoBehaviour
{
    [Header("Player Script")]
    public PlayerHP PlayerHP;

    [Header("Set Healing Ammount")]
    public int HealAmount = 100;

    // On start look for the Players hitbox and get the "PlayerHP" script
    void Start() {
        PlayerHP = GameObject.Find("Player Hitbox").GetComponent<PlayerHP>();
    }

    // This occures when the trigger is pulled while holding on to a healing potion
    // This causes the player to heal and then destroy the healing potion
    public void HealingEvent()
    {
        // Debug.Log(HealAmount);
        // "HealAmount" is set in the Headers above
        PlayerHP.Heal(HealAmount);
        Destroy(this.gameObject);
    }

}

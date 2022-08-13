using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public PlayerHP PlayerHP;
    public int HealAmount = 100;

    void Start() {
        PlayerHP = GameObject.Find("Player Hitbox").GetComponent<PlayerHP>();
    }

    public void HealingEvent()
    {
        Debug.Log(HealAmount);
        PlayerHP.Heal(HealAmount);
        Destroy(this.gameObject);
    }

}

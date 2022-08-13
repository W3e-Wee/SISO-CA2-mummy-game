using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHP : MonoBehaviour
{
    // public Image playerHealthBar;
    public float health;
    public float maxHealth = 100;
    public Image[] hearts;

    float lerpSpeed;
    

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }


    // Update is called once per frame
   private void Update()

    {
        if (health > maxHealth) health = maxHealth;
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();
    }

    void HealthBarFiller()
    {
        for (var i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = !DisplayHealthPoint(health, i);
        }
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ( (pointNumber*10) >= _health);
    }

    public void Damage(float damagePoints)

    {
        if (health > 0)
        {
            health -= damagePoints;
        }
    }

    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
        }
    }
}

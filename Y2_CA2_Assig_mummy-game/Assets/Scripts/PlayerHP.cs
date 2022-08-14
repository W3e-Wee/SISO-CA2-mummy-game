using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    // public Image playerHealthBar;
    public Canvas playerHealthCanvas;
    public Canvas gameOverCanvas;
    public float health;
    public float maxHealth = 100;
    public Image[] hearts;

    float lerpSpeed;
    

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
        playerHealthCanvas = GameObject.Find("PlayerHealthCanvas").GetComponent<Canvas>();
        // gameOverCanvas = GameObject.Find("GameOverCanvas").GetComponent<Canvas>();
        // gameOverCanvas.enabled = false;
    }


    // Update is called once per frame
   private void Update()

    {
        if (health > maxHealth) health = maxHealth;
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();

        if (health <= 0)
        {
            gameOverCanvas.enabled = true;
            playerHealthCanvas.enabled = false;
            Time.timeScale = 0;
            new WaitForSeconds(5f);
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
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

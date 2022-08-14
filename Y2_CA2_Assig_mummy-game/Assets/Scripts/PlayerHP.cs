using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle Player HP
//---------------------------------------------------------------------------------
public class PlayerHP : MonoBehaviour
{
    public Canvas playerHealthCanvas;
    public Canvas gameOverCanvas;
    public float health;
    public float maxHealth = 100;
    public Image[] hearts;

    private bool rollCredits = false;
    // Sets the speed at which the HP bar moves
    float lerpSpeed;
    
    // Start is called before the first frame update
    private void Start()
    {
        // sets health to max health
        health = maxHealth;
        // gets the player HP bar
        playerHealthCanvas = GameObject.Find("PlayerHealthCanvas").GetComponent<Canvas>();
        playerHealthCanvas = GameObject.Find("DeadCanvas").GetComponent<Canvas>();
    }


    // Update is called once per frame
   private void Update()

    {
        if (health > maxHealth) health = maxHealth;
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();

        // if HP is zero or less, game over
        if (health <= 0)
        {
            if (rollCredits == false)
            {
                Debug.Log("Game Over");
                StartCoroutine(GameOver());
                rollCredits = true;
            }
        }
    }

    IEnumerator GameOver()
    {
        // show game over canvas
        gameOverCanvas.gameObject.SetActive(true);
        playerHealthCanvas.gameObject.SetActive(false);

        // wait 5 seconds before returning to menu
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    // Dynamically show how much HP is left
    void HealthBarFiller()
    {
        for (var i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = !DisplayHealthPoint(health, i);
        }
    }

    // Dynamically show how much HP is left
    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ( (pointNumber*10) >= _health);
    }

    // if player is damaged call this
    public void Damage(float damagePoints)
    {
        if (health > 0)
        {
            health -= damagePoints;
        }
    }

    // if player is healed call this
    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
        }
    }
}

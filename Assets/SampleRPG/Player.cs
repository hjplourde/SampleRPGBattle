using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int PlayerHealth;
    public int Potions = 2;
    public int Damage = 5;
    public int ChargeMult = 1;
    public Image PlayerBG;
    public Image EnemyBG;
    public Enemy enemy;
    public Slider PlayerHealthBar;
    public TMP_Text CombatLog;
    public TMP_Text HealthNum;

    public bool Turn;
    //Will determine player and enemy turns

    public AudioSource SwordHit;

    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        //Finds the object with the Enemy Script

        if (enemy != null)
        {
            Debug.Log("Enemy Health: " + enemy.EnemyHealth);
        }
        else
        {
            Debug.LogError("Enemy script not found!");
        }
        //Checks to see if the object was found

        PlayerHealthBar.value = PlayerHealth;
        //Maxes out the healthbar

        Turn = true;
        //Starts the player's turn

        CombatLog.text = "You face Fallen Templar Elliot Pierce";

        HealthNum.text = PlayerHealth + "/20";
    }

    void Update()
    {
        PlayerBG.color = Color.Lerp(PlayerBG.color, new Color(255f / 255f, 255f / 255f, 255f / 255f), Time.deltaTime);
        //Interpolation, you have an "a" a "b" and the percentage change between the two, which can be deltaTime for smooth transitions

        PlayerHealthBar.value = PlayerHealth;
        HealthNum.text = PlayerHealth + "/20";
        //Keeps health data updated

        if (PlayerHealth <= 0)
        {
            SceneManager.LoadScene(1);
        }
        //Loads the defeat screen when killed
    }

    public void Attack()
    {
        if (Turn == true)
        {
            enemy.EnemyHealth -= (Damage*ChargeMult);
            EnemyBG.color = Color.red;
            CombatLog.text = "You struck Elliot for " + Damage * ChargeMult + " damage";
            //Changes text to show how much damage you did
            ChargeMult = 1;
            //Resets charge
            SwordHit.Play();
            Turn = false;
        }
        else
        {
            CombatLog.text = "It isn't your turn!";
        }
    }

    public void Charge()
    {
        if (Turn == true)
        {
            ChargeMult = 2;
            PlayerBG.color = Color.green;
            CombatLog.text = "Damage doubled next turn!";
            Turn = false;
        }
        else
        {
            CombatLog.text = "It isn't your turn!";
        }
    }

    public void Heal()
    {
        if (Turn == true)
        {
            if (Potions > 0)
            {
                PlayerHealth += 7;
                //Could expand this by making it a random range, but all the ints would need to be converted to floats
                Potions -= 1;
                CombatLog.text = "Healed a portion of damage" + "\nPotions remaining: " + Potions;
                PlayerBG.color = Color.green;
                ChargeMult = 1;
                Turn = false;
            }
            else
            {
                CombatLog.text = "You have no potions!";
            }
        }
        else
        {
            CombatLog.text = "It isn't your turn!";
        }
    }

}

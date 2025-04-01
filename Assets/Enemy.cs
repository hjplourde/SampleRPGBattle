using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int EnemyHealth = 25;
    public int Potions = 1;
    public int Damage = 4;
    public int ChargeMult = 1;
    public Image EnemyBG;
    public Image PlayerBG;
    public Player player;
    public Slider EnemyHealthBar;
    public TMP_Text CombatLog;
    public TMP_Text EnemyHealthNum;

    public bool Charge;

    public AudioSource SwordHit;

    private float timer = 2f;


    void Start()
    {
        player = FindObjectOfType<Player>();
        //Finds the object with the Enemy Script

        if (player != null)
        {
            Debug.Log("Player Health: " + player.PlayerHealth);
        }
        else
        {
            Debug.LogError("Enemy script not found!");
        }
        //Checks to see if the object was found

        EnemyHealthBar.value = EnemyHealth;
        //Maxes out Elliot's healthbar

        EnemyHealthNum.text = EnemyHealth + "/25";
    }

    void Update()
    {

        EnemyBG.color = Color.Lerp(EnemyBG.color, new Color(255f / 255f, 255f / 255f, 255f / 255f), Time.deltaTime);
        //Interpolation, you have an "a" a "b" and the percentage change between the two, which can be deltaTime for smooth transitions
        EnemyHealthBar.value = EnemyHealth;
        EnemyHealthNum.text = EnemyHealth + "/25";
        //Keeps health data updated

        if (EnemyHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
        //Loads the victory screen upon defeat


        if (player.Turn == false)
        {
            timer -= Time.deltaTime;
            //Decreases timer by one so that there is a short buffer between turns
            if (timer <= 0f)
            {
                EnemyAction();
                timer = 2f;
                //Resets the timer after calling the function
                
            }
            
        }

    }

    void EnemyAction()
    {
        if (EnemyHealth < 12 && Potions > 0)
        {
                EnemyHealth += 7;
                //Could expand this by making it a random range, but all the ints would need to be converted to floats
                Potions -= 1;
                CombatLog.text = "Elliot healed a portion of damage";
                EnemyBG.color = Color.green;
                ChargeMult = 1;
                Charge = false;
                //Makes sure that charge is false if it was wasted to heal during this turn
                player.Turn = true;
        }
        else if (Charge == true)
        {
            player.PlayerHealth -= (Damage*ChargeMult);
            SwordHit.Play();
            //Might play a sound, we'll see
            PlayerBG.color = Color.red;
            CombatLog.text = "Elliot slices you for " + (Damage * ChargeMult) + " damage";
            ChargeMult = 1;
            Charge = false;
            //Resets the charge boolean so it can be used again
            player.Turn = true;
        }
        else if (player.PlayerHealth >= 11)
            //Please please please work why isn't this working in the build
        {
            Debug.Log(player.Turn);
            //Checks if any of this works
            Charge = true;
            ChargeMult = 2;
            CombatLog.text = "Elliot is taking an offensive stance";
            player.Turn = true;
        }
        else if (player.PlayerHealth <= 10)
        {
            player.PlayerHealth -= (Damage * ChargeMult);
            SwordHit.Play();
            PlayerBG.color = Color.red;
            CombatLog.text = "Elliot slices you for " + (Damage * ChargeMult) + " damage";
            Charge = false;
            //Resets charge so that Elliot can attack again
            ChargeMult = 1;
            player.Turn = true;
        }
    }
}

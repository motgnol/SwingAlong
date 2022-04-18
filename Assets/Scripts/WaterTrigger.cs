using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    //private bool isInWater;
    private int health;

    void Update()
    {
        health = GameManager.Instance.Health;
    }

    

    void OnTriggerEnter2D(Collider2D other)
    {
         if(other.tag == "Player")
        {
            GameManager.Instance.Health = health - 1;
            health = GameManager.Instance.Health;

            //Play "loss" sound
            if(health > 0)
            {
                GameManager.Instance.PlaySoundsEffects("failClip");
            }
            else if(health <= 0)
            {
                GameManager.Instance.PlaySoundsEffects("gameOverClip");
            }            
        }
    }

    void OnTriggerStay2D (Collider2D other) 
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.IsInWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
         if(other.tag == "Player")
        {
            GameManager.Instance.IsInWater = false;
        }
    }
}

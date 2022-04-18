using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimbScript : MonoBehaviour
{
    private bool canClimb = false;


    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canClimb = true;
            GameManager.Instance.CanClimb = canClimb;

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canClimb = false;
            GameManager.Instance.CanClimb = canClimb;

        }

    }
    
}

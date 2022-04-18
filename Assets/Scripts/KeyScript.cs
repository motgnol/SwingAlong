using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private bool hasKey = false;

    void Start()
    {
        GameManager.Instance.HasKey = hasKey;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Display key as gained asset
            GameManager.Instance.HasKey = true;
            GameManager.Instance.PlaySoundsEffects("gotBonusClip");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEndScript : MonoBehaviour
{   
    private TMP_Text wellDoneText;
    private bool hasKey;

    void Start()
    {
        wellDoneText = GameObject.FindGameObjectWithTag("WellDoneText").GetComponent<TMP_Text>();
    }
    void Update()
    {
        hasKey = GameManager.Instance.HasKey;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.tag == "Player" && hasKey)
       {
            wellDoneText.enabled = true;
            GameManager.Instance.PlaySoundsEffects("nextLevelClip");
           

       } 

    }

}

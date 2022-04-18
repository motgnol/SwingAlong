using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] private float waterSpeed = 2f;
    private SpriteRenderer thisRenderer;
    private Transform waterImage;
    private Vector3 resetLocation;
    


    void Start()
    {
        waterImage = GetComponent <Transform>();
        resetLocation = GameManager.Instance.WaterResetLocation;
        thisRenderer = GetComponent<SpriteRenderer>();

    }

    
    void Update()
    {
        waterImage.Translate(-(waterSpeed * Time.deltaTime), 0f, 0f);

        if(waterImage.position.x <= -(resetLocation.x))
        {
            waterImage.position = new Vector3(resetLocation.x, -10.45f, 0f);

        }
        
    }
    
}

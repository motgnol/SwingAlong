using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameBoundriesScript : MonoBehaviour
{
    private Transform _topBoundry;
    private Transform _leftBoundry;
    private Transform _rightBoundry;

    private Transform[] _boundries;

    

    
    void Start()
    {
        float _screenHalfHeight = GameManager.Instance.ScreenHalfHeightWorldUnits;
        float _screenHalfWidth = GameManager.Instance.ScreenHalfWidthWorldUnits;

        _topBoundry = GameObject.FindGameObjectWithTag("TopBoundry").GetComponent<Transform>();
        _leftBoundry = GameObject.FindGameObjectWithTag("LeftBoundry").GetComponent<Transform>();
        _rightBoundry = GameObject.FindGameObjectWithTag("RightBoundry").GetComponent<Transform>();


        _boundries = new Transform[] {_topBoundry, _leftBoundry, _rightBoundry};

        foreach(Transform boundry in _boundries)
        {
            if(boundry.tag == "TopBoundry")
            {
                _topBoundry.position = new Vector3(0f, (_screenHalfHeight + _topBoundry.localScale.y / 2), 0f);
            }

            if(boundry.tag == "LeftBoundry")
            {
                _leftBoundry.position = new Vector3(-(_screenHalfWidth + _leftBoundry.localScale.x / 2), 0f, 0f);
            }

            if(boundry.tag == "RightBoundry")
            {
                _rightBoundry.position = new Vector3((_screenHalfWidth + _rightBoundry.localScale.x / 2), 0f, 0f);
            }
        }
    }
    
}

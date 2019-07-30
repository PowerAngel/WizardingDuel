using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetMovement : MonoBehaviour
{

    public double moveTime = 2;
    public int speed = 1;

    DateTime _lastEventTime;

    // Start is called before the first frame update
    void Start()
    {
        _lastEventTime = DateTime.UtcNow;
    }

    // Update is called once per frame
    void Update()
    {
        if ((DateTime.Now - _lastEventTime) < TimeSpan.FromSeconds(moveTime))
        {
            transform.position += transform.right * Time.deltaTime;
        }
        else if((DateTime.Now - _lastEventTime) < TimeSpan.FromSeconds(moveTime*2))
        {
            transform.position -= transform.right * Time.deltaTime;
        }
        else
        {
            _lastEventTime = DateTime.Now;
        }
    }
}

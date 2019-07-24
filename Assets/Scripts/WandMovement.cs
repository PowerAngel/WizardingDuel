using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = targetPos;
        }
    }
}

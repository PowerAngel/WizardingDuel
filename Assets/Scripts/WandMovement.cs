using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandMovement : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("pressed left click");
            //var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = targetPos;
            //text.text = "Mousepos: " + targetPos.ToString();
        }
    }
}

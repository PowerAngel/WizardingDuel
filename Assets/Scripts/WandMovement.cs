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
        if (Input.GetMouseButton(0))
        {
            Vector3 targetPos = Input.mousePosition;
            
            Debug.Log("pressed left click" + targetPos.ToString());
            text.text = "mouse: " + targetPos.ToString();

            //Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // kamerans position
            //var targetPos = Event.current.mousePosition;
            //transform.position = targetPos;
        }

    }
}

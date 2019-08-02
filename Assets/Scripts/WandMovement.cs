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

        Vector3 a = new Vector3(1, 2, 4);
        Vector3 b = new Vector3(1, 2, 3);
        Debug.Log("vectortes: " + a.magnitude.ToString());

        /*
        Vector3 pmin = new Vector3((float)3, (float)7, (float)10);
        Vector3 pmax = new Vector3((float)5, (float)6, (float)8);

        //subtract the mean position from all poses

        //divide all points by (pmax-pmin)

        var meanPmin = (pmin[0] + pmin[1] + pmin[2]) / 3.0f;
        var meanPmax = (pmax[0] + pmax[1] + pmax[2]) / 3.0f;
        var n = meanPmax - meanPmin;
        Debug.Log("pmin: " + pmin.ToString());
        Debug.Log("pmax: " + pmax.ToString());

        Debug.Log("meanpmin: " + meanPmin.ToString());
        Debug.Log("meanpmax: " + meanPmax.ToString());
        Debug.Log("n: " + n.ToString());
        pmin /= n;
        pmax /= n;
        Debug.Log("pmin: " + pmin.ToString());
        Debug.Log("pmax: " + pmax.ToString());*/
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

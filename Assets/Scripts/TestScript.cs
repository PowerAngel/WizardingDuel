using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float tnow = UnityEngine.Time.time;
        Aff3d T = new Aff3d(0,0,0,0,0,0);

        text.text = "Time now: " + tnow.ToString()+ "and output: "+T.ToString();
        

    }
}

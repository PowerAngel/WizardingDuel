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
        //Aff3d T = new Aff3d(0,0,0,0,0,0);
        TrajectoryMatcher tmatch = new TICPMatcher();
        Trajectory traj1 = new Trajectory();
        Trajectory traj2 = new Trajectory();
       for (int i = 0; i < 10; i++){
            Aff3d T = new Aff3d(i, i, i, 0, 0, 0);
            traj1.push_back(T);
            T.Translation += new Vector3(0, 0, (float)0.5);
            traj2.push_back(T);
        }
        Aff3d Talign = new Aff3d();
        double score = 0;
        tmatch.Match(traj1, traj2, out Talign, out score);
        Debug.Log("traj1: " + traj1.ToString());


    }

    // Update is called once per frame
    void Update()
    {
        float tnow = UnityEngine.Time.time;
        
  
        //tmatch.Match(traj1, traj2, out Talign, out score);
       
        //text.text = "Time now: " + tnow.ToString()+ "and output: "+T.ToString();
        

    }
}

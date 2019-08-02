using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text text;
    public GameObject spellParticlePlot;
    // Start is called before the first frame update

    void Plot(Trajectory target, Trajectory src, Trajectory registered)
    {
        
        List<Aff3d> posesA_ = target.GetTrajectory();

        foreach (Aff3d pose in posesA_)
        {
            GameObject gameObject = Instantiate(spellParticlePlot);
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            gameObject.transform.position = new Vector3(pose.Translation.x, pose.Translation.y, pose.Translation.z);
            Vector3 rotation = pose.Rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(rotation);
            
        }

        
        List<Aff3d> posesB_ = src.GetTrajectory();

        foreach (Aff3d pose in posesB_)
        {
            GameObject gameObject = Instantiate(spellParticlePlot);
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            gameObject.transform.position = new Vector3(pose.Translation.x, pose.Translation.y, pose.Translation.z);
            Vector3 rotation = pose.Rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(rotation);
        }

        List<Aff3d> posesC_ = registered.GetTrajectory();

        foreach (Aff3d pose in posesC_)
        {
            GameObject gameObject = Instantiate(spellParticlePlot);
            gameObject.GetComponent<Renderer>().material.color = Color.green;
            gameObject.transform.position = new Vector3(pose.Translation.x, pose.Translation.y, pose.Translation.z);
            Vector3 rotation = pose.Rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(rotation);
        }
    }
    void Start() { 
    Trajectory src = new Trajectory("data/wand_SmallHeart.txt");
    Trajectory target = new Trajectory("data/wand_LargeHeart.txt");
    Trajectory registered = new Trajectory("data/wand_SmallHeart.txt");
    Aff3d Toffset = new Aff3d(0, 0, 0.5f, 0, 0, 0);
    registered.Transform(Toffset);
        
     Plot(target, src, registered);

    //spellParticlePlot = new GameObject();

    //Aff3d T = new Aff3d(0,0,0,0,0,0);
    //TrajectoryMatcher tmatch = new TICPMatcher();



    /*
    Trajectory traj1 = new Trajectory();
    Trajectory traj2 = new Trajectory();
   for (int i = 0; i < 10; i++){
        Aff3d T = new Aff3d(i, i, i, 0, 0, 0);
        traj1.push_back(T);
        Aff3d T2 = new Aff3d(i+(float)0.5, i+(float)0.5, i + (float)0.5, 0, 0, 0);
        traj2.push_back(T2);
    }*/
    Aff3d Talign = new Aff3d();
        double score = 0;
        
        //tmatch.Match(traj1, traj2, out Talign, out score);
        




    }

    // Update is called once per frame
    void Update()
    {
        float tnow = UnityEngine.Time.time;
        
  
        //tmatch.Match(traj1, traj2, out Talign, out score);
       
        //text.text = "Time now: " + tnow.ToString()+ "and output: "+T.ToString();
        

    }
}

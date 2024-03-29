﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text text;
    public GameObject spellParticlePlot;
    // Start is called before the first frame update

    void Plot(Trajectory target, Trajectory src, Trajectory registered, Aff3d offset)
    {

        List<Aff3d> posesA_ = target.GetTrajectory();

        foreach (Aff3d pose in posesA_)
        {
            GameObject gameObject = Instantiate(spellParticlePlot);
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            gameObject.transform.position = new Vector3(pose.Translation.x, pose.Translation.y, pose.Translation.z) + offset.Translation;
            Vector3 rotation = pose.Rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(rotation);

        }


      /*  List<Aff3d> posesB_ = src.GetTrajectory();

        foreach (Aff3d pose in posesB_)
        {
            GameObject gameObject = Instantiate(spellParticlePlot);
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            gameObject.transform.position = new Vector3(pose.Translation.x, pose.Translation.y, pose.Translation.z) + offset.Translation;
            Vector3 rotation = pose.Rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(rotation);
        }*/

        List<Aff3d> posesC_ = registered.GetTrajectory();

        foreach (Aff3d pose in posesC_)
        {
            GameObject gameObject = Instantiate(spellParticlePlot);
            gameObject.GetComponent<Renderer>().material.color = Color.green;
            gameObject.transform.position = new Vector3(pose.Translation.x, pose.Translation.y, pose.Translation.z) + offset.Translation;
            Vector3 rotation = pose.Rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(rotation);
        }
    }
    void Start()
    {
        Trajectory src = new Trajectory("data/wand_SmallHeart.txt");
        Trajectory target  = new Trajectory("data/wand_SmallHeart.txt"); //= new Trajectory("data/wand_LargeHeart.txt");
        Trajectory registered = new Trajectory("data/wand_LargeHeart.txt");

        //src.Normalize();
        //registered.Center();
        
        Aff3d T = new Aff3d(0.0f, 0.0f, 0.0f, 0.4f, 0.4f, 0.4f);
        registered.Transform(T);
        
        //target.RescaleByDimension();
        //src.Center();
        //src.RescaleByDimension();
        //registered.Normalize();
        //registered.Transform(Toffset);
        Aff3d vis_offset = new Aff3d(1.0f, 2.5f, -1.5f, 0, 0, 0);

        TrajectoryMatcher tmatch = new ICPMatcher();

        Aff3d Talign = new Aff3d();
        double score = 0;

        //src.Transform(Aff3d.Identity());
        //Plot(target, src, src, vis_offset);


        /*for (int i = 0; i < 1; i++)
        {
            if (tmatch.Match(target, registered, out Talign, out score))
            {
                registered.Transform(Talign);
                
            }
        }*/
        target.Rescale();
        
        registered.Rescale();
        //Plot(target, src, registered, vis_offset);

        for (int i = 0; i < 10; i++)
        {
            if (tmatch.Match(target, registered, out Talign, out score))
            {
                registered.Transform(Talign);
                
            }
        }
        Plot(target, src, registered, vis_offset);








    }

    // Update is called once per frame
    void Update()
    {
        float tnow = UnityEngine.Time.time;


        //tmatch.Match(traj1, traj2, out Talign, out score);

        //text.text = "Time now: " + tnow.ToString()+ "and output: "+T.ToString();


    }
}

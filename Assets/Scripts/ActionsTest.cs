using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;


public class ActionsTest : MonoBehaviour {

    public SteamVR_Input_Sources handType; //1
    public SteamVR_Action_Boolean teleportAction; //2
    public SteamVR_Action_Boolean grabAction; //3
    public WritePoses wand_tip_writer, hand_writer;

    private LineRenderer lineRenderer;

    private ArrayList positionsArray = new ArrayList();

    ParticleSystem sparks;
    
    // Update is called once per frame

    private void Start()
    {
        //hand_writer = new WritePoses("hand.txt");
        wand_tip_writer = new WritePoses("wand.txt");
        sparks = GetComponent<ParticleSystem>();
        sparks.emissionRate = 500;
    }

    void Update () {

        //GameObject TTip = new GameObject();

        //TTip.transform.position = GetWandTip(transform).position;
        //TTip.transform.rotation = GetWandTip(transform).rotation;
        Vector3 wand_tip = new Vector3(0, 0, 0);
        Quaternion Qwand_tip = Quaternion.identity;
        float bergs_wand = (float)(12 * 3 / 4 * 2.54 / 100); //elder wand length

        GetWandTip(transform, out Qwand_tip, out wand_tip, bergs_wand);
       

        if (GetTeleportDown())
        {
            print("Teleport " + handType);
        }
        if (GetGrab())
        {
            print("Grab " + handType);
            //sparks.emissionRate = 500;
            sparks.Play();

            /*float x = tansform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            float ex = transform.localEulerAngles.x * Mathf.Deg2Rad;
            float ey = transform.localEulerAngles.y * Mathf.Deg2Rad;
            float ez = transform.localEulerAngles.z * Mathf.Deg2Rad;*/


            if (hand_writer != null)
            {

                float x = wand_tip.x;
                float y = wand_tip.y;
                float z = wand_tip.z;
                float ex = Qwand_tip.eulerAngles.x * Mathf.Deg2Rad;
                float ey = Qwand_tip.eulerAngles.y * Mathf.Deg2Rad;
                float ez = Qwand_tip.eulerAngles.z * Mathf.Deg2Rad;
                print("Wrote to hand.txt: " + hand_writer.WritePose(x, y, z, ex, ey, ez));
                //print("Wrote to hand.txt");
            }
            else
            {
                print("hand_writer is null");
            }

            if(wand_tip_writer != null)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                float ex = transform.localEulerAngles.x * Mathf.Deg2Rad;
                float ey = transform.localEulerAngles.y * Mathf.Deg2Rad;
                float ez = transform.localEulerAngles.z * Mathf.Deg2Rad;
                print("Wrote to wand.txt: " + wand_tip_writer.WritePose(x, y, z, ex, ey, ez));              
            }
            else
            {
                print("wand_tip_writer is null");
            }
        }
        else
        {
            //sparks.emissionRate = 0;
            sparks.Stop();
        }
        
	}

    public bool GetTeleportDown()
    {
        return teleportAction.GetLastStateDown(handType);
    }

    public bool GetGrab()
    {
        return grabAction.GetState(handType);
    }

    
    void GetWandTip(Transform Thand, out Quaternion rot, out Vector3 Pos , float length=0)
    {
        //GameObject Twand = new GameObject();
        Quaternion Qwand = Quaternion.identity;
        Vector3 wand_offset = new Vector3(0, 0, length);
  
        rot = Thand.rotation * Qwand;
        Pos = Thand.rotation * wand_offset + Thand.transform.position;
        //return Ttip.transform;
    }
}


public class WritePoses
{
    public WritePoses(string filename)
    {
        writer = new StreamWriter(filename); //open
    }
    ~WritePoses()
    {
        writer.Close();
    }

    public string WritePose(float x, float y, float z, float ex, float ey, float ez)
    {
        string pose_str = x.ToString() + " " + y.ToString() + " " + z.ToString() + " " + ex.ToString() + " " + ey.ToString() + " " + ez.ToString();
        writer.WriteLine(pose_str);

        return pose_str;
    }
    public StreamWriter writer;
}
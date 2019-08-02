using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.IO;
using System;
using System.Globalization;

public class Trajectory //: MonoBehaviour
{
    public Trajectory()
    {
        poses_ = new List<Aff3d>();
    }
    public Trajectory(string path) : this()
    { //load trajectory from text file. file contain a number of poses each separated by a new line. each pose is represented by 6 values, x y z ex ey ez>
      //load poses from file
        ReadPosesFromFile(path);
    }

    public List<Aff3d> GetTrajectory()
    {
        return poses_;
    }

    public void push_back(Aff3d T)
    {
        //Debug.Log("adding : " + T.ToString());
        poses_.Add(T);
        //add T to poses
    }
    public void Transform(Aff3d Toffset)
    {
        List<Aff3d> poses_offset = new List<Aff3d>();
        foreach(Aff3d p in poses_)
        {
            Aff3d T = Toffset * p;
            poses_offset.Add(T);
            
        }
        poses_ = poses_offset;
    }
    public void Normalize(){
        Vector3 pmin = new Vector3((float)Min(0), (float)Min(1), (float)Min(2));
        Vector3 pmax = new Vector3((float)Max(0), (float)Max(1), (float)Max(2));


        //subtract the mean position from all poses
        //divide all poses by (pmax-pmin)

        /*var meanPmin = (pmin[0] + pmin[1] + pmin[2]) / 3.0f;
        var meanPmax = (pmax[0] + pmax[1] + pmax[2]) / 3.0f;

        
        var n = meanPmax - meanPmin;
        Debug.Log("pmin: " + pmin.ToString());
        Debug.Log("pmax: " + pmax.ToString());
        Debug.Log("n: " + n.ToString());
        pmin /= n;
        pmax /= n;
        Debug.Log("pmin: " + pmin.ToString());
        Debug.Log("pmax: " + pmax.ToString());
        */
        Vector3 size_cloud = new Vector3();
        size_cloud = pmax - pmin;
        Vector3 tmp = new Vector3();
        foreach (Aff3d p in poses_){
            tmp = p.Translation;
            tmp.x = tmp.x / size_cloud.x;
            tmp.y = tmp.y / size_cloud.y;
            tmp.z = tmp.z / size_cloud.z;
            p.Translation = tmp;
        }
    }

    public int GetClosest(float index, float window, ref Vector3 target) //Returns index for the closest pose, in poses_, to target.
    {

        /* This is not usefull atm
        * //Finds the pose which has a position nearest "closest.  Return true if any pose was found, otherwise false. Index and window is currently not usefull!    index between 0 and 1, window in percentage of all poses
        bool found = false;
        bool first = true;
        int i_idx = (int)(index / (float)poses_.Count);
        int i_win_size = (int)(window / (float)poses_.Count);
        for(int i = i_idx - i_win_size; i <= i_idx + i_win_size; i++){
            if(i<0 || i > poses_.Count){

            }
        }*/
        //bool anyPoses = false;
        Vector3 tmp = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        int idx = -1;
        for(int i = 0;  i < poses_.Count; i++){
            if ((target - poses_[i].Translation).magnitude < tmp.magnitude)
            {
                tmp = target - poses_[i].Translation;
                idx = i;
            }
        }
        return idx;
            
        //return -1; //false, couldn't find any pose.

    }
    public int Size
    {
        get { return poses_.Count; }
    }
    public Matrix<double> PositionMatrix(){

        Matrix<double> m = Matrix<double>.Build.Random(this.Size, 3);
        for(int i = 0; i < this.Size; i++){
            Vector<double> vi = Vector<double>.Build.Dense(3);
            vi[0] = poses_[i].Translation.x;
            vi[1] = poses_[i].Translation.y;
            vi[2] = poses_[i].Translation.z;
            m.SetRow(i, vi);
        }
        return m;
}

        
    private double Min( int dim)
    { //returns the value of the aff3d which has the smallest number om the dim axis (x = dim 0, y=dim1 z = dim 2)
        if(dim<0 ||dim > 2)
        {
            Debug.LogWarning("wrong dim");
            return 0;
        }
        double xmin = double.MaxValue;
        foreach( Aff3d p in poses_){
            if (p.Translation[dim] < xmin)
                xmin = p.Translation[dim];
        }
        return xmin;
    }
    private double Max( int dim)
    { //returns the value of the aff3d which has the largest number om the dim axis (x = dim 0, y=dim1 z = dim 2)
        double xmax = double.MinValue;
        foreach (Aff3d p in poses_)
        {
            if (p.Translation[dim] > xmax)
                xmax = p.Translation[dim];
        }
        return xmax;
    }

    private Vector3 mean()//get centroid, or equivalently, the mean value per of x y and z.	
    {
        Vector3 vek = new Vector3(0, 0, 0);
        foreach(Aff3d p in poses_)
            vek += p.Translation;

        vek /= poses_.Count;
        return vek;
    }
    public override string ToString()
    { // print all poses
        string tmp = "";
        foreach(Aff3d p in poses_){
            tmp += p.ToString() + "\n";
        }
        return tmp;
    }
    static public double Distance(Trajectory target, Trajectory src) //not needed atm
    {
        return 0;
    }

    private void ReadPosesFromFile(string path)
    {
        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                Debug.Log("starting while loop");
                while((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(' ');
                    float x = float.Parse(values[0], CultureInfo.InvariantCulture);
                    float y = float.Parse(values[1], CultureInfo.InvariantCulture);
                    float z = float.Parse(values[2], CultureInfo.InvariantCulture);
                    float ex = float.Parse(values[3], CultureInfo.InvariantCulture);
                    float ey = float.Parse(values[4], CultureInfo.InvariantCulture);
                    float ez = float.Parse(values[5], CultureInfo.InvariantCulture);
                    Aff3d aff3d = new Aff3d(x, y, z, ex, ey, ez);
                    poses_.Add(aff3d);
                }             
            }
        }
        catch (Exception e)
        {
            Debug.Log("Could not read file: " + path + ". Error: " + e);
        }
    }

    private List<Aff3d> poses_;
}



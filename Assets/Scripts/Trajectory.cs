using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
public class Trajectory
{
    public Trajectory()
    {
        poses_ = new List<Aff3d>();

    }
    public Trajectory(string path):this()
        
    { //load trajectory from text file. file contain a number of poses each separated by a new line. each pose is represented by 6 values, x y z ex ey ez>
      //load poses from file	
    }
    public void push_back( Aff3d T)
    {
        Debug.Log("adding : " + T.ToString());
        poses_.Add(T);
        //add T to poses
    }
    public void Normalize()
    {


    }
    public int Size
    {
        get { return poses_.Count; }
    }
    public Matrix<double>{

        Matrix<double> m = Matrix<double>.Build.Random(target.Size, 3);
    private double min( uint dim)
    { //returns the value of the aff3d which has the smallest number om the dim axis (x = dim 0, y=dim1 z = dim 2)
        double xmin = double.MaxValue;
        //find smallest
        return xmin;
    }
    private double max( uint dim)
    { //returns the value of the aff3d which has the largest number om the dim axis (x = dim 0, y=dim1 z = dim 2)
        double xmax = double.MinValue;
        //find largest
        return xmax;
    }

    private Vector3 mean()//get centroid, or equivalently, the mean value per of x y and z.	
    {
        Vector3 vek = new Vector3(0, 0, 0);
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

    private List<Aff3d> poses_;
}



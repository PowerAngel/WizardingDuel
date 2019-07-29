using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Trajectory
{
    public Trajectory()
    {

    }
    public Trajectory(string path)
    { //load trajectory from text file. file contain a number of poses each separated by a new line. each pose is represented by 6 values, x y z ex ey ez>
      //load poses from file	
    }
    public void push_back(Aff3d T)
    {
        //add T to poses
    }
    public void Normalize()
    {


    }
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
        return tmp;
    }
    static public double Distance(Trajectory target, Trajectory src) //not needed atm
    {
        return 0;
    }

    private List<Aff3d> poses_;
}



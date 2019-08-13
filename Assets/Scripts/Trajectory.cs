using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.IO;
using System;
using System.Globalization;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;


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
        for(int i=0;i<poses_.Count;i++)
        {
            poses_[i] = Toffset * poses_[i];   
        }
    }
    public Trajectory GetTransformed(Aff3d Toffset)
    { 
        Trajectory transformed = new Trajectory();
        foreach (Aff3d p in poses_)
        {
            transformed.push_back(Toffset * p);
        }
        return transformed;
    }
    public void Center(){
        Vector3 centroid = mean();
        foreach (Aff3d p in poses_)
            p.Translation -= centroid;
    }
    public void RescaleByDimension()
    {
        Vector3 pmin = new Vector3((float)Min(0), (float)Min(1), (float)Min(2));
        Vector3 pmax = new Vector3((float)Max(0), (float)Max(1), (float)Max(2));
        Vector3 size_cloud = new Vector3();
        size_cloud = pmax - pmin;
        Vector3 tmp = new Vector3();
        foreach (Aff3d p in poses_)
        {
            tmp = p.Translation;
            tmp.x = 0.2f * tmp.x / size_cloud.x;
            tmp.y = 0.2f * tmp.y / size_cloud.y;
            tmp.z = 0.2f * tmp.z / size_cloud.z;
            p.Translation = tmp;
        }
    }
    public void Rescale()
    {
        Center();
         Matrix<double> A = PositionMatrix();
         Matrix<double> cov = 1.0f / ((float)A.RowCount ) *A.Transpose() * A;
        Evd<double> eigen = cov.Evd();
        MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex> eigenvals = eigen.EigenValues;
        double l1 = eigenvals[0].Real;//1 / Math.Sqrt(eigenvals[0].Real);
        double l2 = eigenvals[1].Real;//1 / Math.Sqrt(eigenvals[1].Real);
        double l3 = eigenvals[2].Real; // / Math.Sqrt(eigenvals[2].Real);
        Debug.Log("l1: "+l1+", l2: "+l2+", l3: "+l3);
        double vol = l1 * l2 * l3;
        double max = eigenvals.AbsoluteMaximum().Real;
        Debug.Log("max=" + max);
        //double det = cov.Determinant();

        A = A / Math.Sqrt( max)*0.2;
        Debug.Log(A);
        
        //Debug.Log("determinant:");
        for (int i = 0; i < A.RowCount; i++)
        {
            poses_[i].Translation = new Vector3((float)A[i, 0], (float)A[i, 1], (float)A[i, 2]);
        }
        


        /* Matrix<double> A = PositionMatrix();
         Matrix<double> cov = 1.0f / ((float)A.RowCount ) *A.Transpose() * A;

         
         Debug.Log(cov.ToString());
         Evd<double> eigen = cov.Evd();
         Matrix<double> m = eigen.EigenVectors;
         Matrix<double> points = m * A * m.Inverse();

         points = points * cov.Determinant() / 10;
         points = m * points * m.Inverse();
         for (int i = 0; i < points.RowCount; i++)
         {
             poses_[i].Translation =new Vector3((float)points[i, 0], (float)points[i, 1], (float)points[i, 2]);
         }*/
        /*double det = cov.Determinant();
        cov_rotated = cov_rotated / det;
        Matrix<double> cov_recovered = m * cov_rotated * m.Inverse();*/
        //Debug.Log(cov_recovered.ToString());


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
        return 0;
 
    }
    public int GetClosest( Vector3 target) //Finds the pose which has a position nearest "closest.  Return true if any pose was found, otherwise false. 
    {
        Vector3 tmp = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        int idx = -1;
        for (int i = 0; i < poses_.Count; i++){
            if ((target - poses_[i].Translation).sqrMagnitude < tmp.sqrMagnitude){
                tmp = target - poses_[i].Translation;
                idx = i;
            }
        }
        return idx;
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
    public Matrix<double> PositionMatrix(List<int> idx, bool select_by_indices = false) //returns a position matrix filtered from -1 indexes. if select by indices is true,  the values in idx will determine which elements to insert in the matrix
    {
        int size_m = 0;
        foreach( int i in idx){
            if (i != -1)
                size_m++;
        }
        Matrix<double> m = Matrix<double>.Build.Random(size_m, 3);
        int current_row = 0;
        for (int i = 0; i < idx.Count; i++){
            if (idx[i] == -1)
                continue;
            int point = i;
            if (select_by_indices)
                point = idx[i]; //use whats in the idx rather than just i
           
            Vector<double> vi = Vector<double>.Build.Dense(3);
            vi[0] = poses_[point].Translation.x;
            vi[1] = poses_[point].Translation.y;
            vi[2] = poses_[point].Translation.z;
            m.SetRow(current_row++, vi);
        }
        return m;
    }

    public Aff3d GetPose(int i)
    {
        if (i >= 0 && i < poses_.Count)
        {
            return poses_[i];
        }
        else return Aff3d.Identity();

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
                //Debug.Log("starting while loop");
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



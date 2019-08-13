using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;
public class TrajectoryMatcher 
{
    public TrajectoryMatcher() 
    {
        //spellParticlePlot = new GameObject();
    }
    public virtual bool Match(Trajectory target, Trajectory source, out Aff3d T, out double score)
    {
        T = Aff3d.Identity();
        score = 0;
        return true;
    }
    /*public void SetGameObj(ref GameObject go)
    {
        spellParticlePlot = go;
    }*/
    public void TestPlot()
    {

        //private GameObject[] Targets;



        /*        GameObject tempObject = Instantiate(myPrefab, new Vector3(-6.25f, 1.25f, -7), Quaternion.identity);

                Vector3 direction = tempObject.transform.position - Camera.main.transform.position;
                tempObject.transform.rotation = Quaternion.LookRotation(direction);

                Targets.Add(tempObject);*/
        

 
    }
    public void SVDAlign( Matrix<double> target,  Matrix<double> source, ref Aff3d T) //target
    {
        
        if(target.RowCount != source.RowCount || source.RowCount ==0)
        {
            //ERROR
            Debug.LogError("Rows in matrices not consistent inside alignment");
            return;
        }

        Vector<double> c0 = Vector<double>.Build.Dense(3);
        Vector<double> c1 = Vector<double>.Build.Dense(3);
        
        
        for (int i=0;i< target.RowCount; i++){ //calculate centroid
            c0 += target.Row(i);
            c1 += source.Row(i);
        }
        c0 *= (1.0 / ((double)target.RowCount)); //normalize
        c1 *= (1.0 / ((double)target.RowCount));


        Matrix<double> tar = target;
        Matrix<double> src = source;
        
        for (int i = 0; i < tar.RowCount; i++){
            tar.SetRow(i, tar.Row(i) - c0);
            src.SetRow(i, src.Row(i) - c1);
        }
        //Debug.Log("NO CENTROID " + tar.ToString());

        Matrix<double> H = tar.Transpose()* src;
        
        var solved = H.Svd(true);
        Matrix<double> V = solved.VT.Transpose();
        Matrix<double> Vt = solved.VT;
        Matrix<double> U = solved.U;
        //Matrix<double> det_mat = Matrix<double>.Build.DenseIdentity(3,3);
        //det_mat[2, 2] = (Vt.Transpose() * U.Transpose()).Determinant();
        Matrix<double> R = Vt.Transpose()*U.Transpose();
        
        double det = R.Determinant();

        if (det < 0.0){
            Vt.SetRow(2, -Vt.Row(2) );
            //det_mat[2, 2] = (Vt.Transpose() * U.Transpose()).Determinant();
            R = Vt.Transpose()  * U.Transpose();
            //Debug.Log("Incorrect");
        }

       // Debug.Log("correct");


        Vector<double> tr = -(c1 - R * c0);
        //Debug.Log("offset T: " + tr);
        //Debug.Log("offset R: " + R.ToString());

        Quaternion q = Aff3d.RotToQuaternion(R.Inverse());
        q.Normalize();
        T.Rotation = q;
        //Debug.Log("EULER R: " + q.eulerAngles.ToString());
        T.Translation = new Vector3((float)tr[0], (float) tr[1], (float)tr[2]);

    }
    
    private double score_th_success_;
    

}

public class ICPMatcher : TrajectoryMatcher
{
    public ICPMatcher()
    {
        Debug.Log("constructor matcher");
    }
    /*void TimeProjectionCorrespondance(Trajectory target, Trajectory source)
    {

    }*/
    void AssiciateByNearest(Trajectory target, Trajectory source, ref List<int> associations)
    {
        if (associations == null)
            associations = new List<int>();
        else
            associations.Clear();
        
        for (int i = 0; i < source.Size; i++){   
            int index = target.GetClosest(source.GetPose(i).Translation);
            associations.Add(index);
        }
    }

    public void FilterOutliers(ref List<int> ass_idx, float perc_outlirs)
    {
        

    }

    public override bool Match(Trajectory target, Trajectory source, out Aff3d T, out double score)
    {
        
        score = 0;
        T = Aff3d.Identity();
        //Debug.Log("matched");
        
        List<int> ass_idx = new List<int>();
        AssiciateByNearest( target,  source, ref ass_idx);
        Debug.Log("ASS:");
        Debug.Log(ass_idx);
        
     //   FilterOutliers(ref ass_idx, 0.1f);

        Matrix<double> tar = target.PositionMatrix(ass_idx, true);
        Matrix<double> src = source.PositionMatrix(ass_idx, false);
        Debug.Log("tar size: " + tar.ToString());
        Debug.Log("src size: " + src.ToString());

        SVDAlign(tar,  src, ref T);
        Debug.Log("MATCH OUTPUT: " + T.ToString());
        //source.Transform(T);


        
        return true;

        
        
        // determine correspondence by time

        //use svd to minimize least squares

        //calculate symmetric distance

        //double score = Trajectory::Distance(target, src)



    }
    private double dt_corr_srch_ = 0.1;
}

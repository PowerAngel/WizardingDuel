using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;
public class TrajectoryMatcher : MonoBehaviour
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
    public void SVDAlign(in Matrix<double> target,  Matrix<double> source, ref Aff3d T) //target
    {
        
        if(target.RowCount != source.RowCount)
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
        //Debug.Log("c0: " + c0.ToString());
        //Debug.Log("c1: " + c1.ToString());

        c0 *= (1.0 / ((double)target.RowCount)); //normalize
        c1 *= (1.0 / ( (double)target.RowCount));
        //Debug.Log("c0: "+c0[0]+" " + c0[1]+ " "+c0[2]);

        Matrix<double> tar = target;
        Matrix<double> src = source;
        for(int i = 0; i < tar.RowCount; i++){
            tar.SetRow(i, tar.Row(i) - c0);
            src.SetRow(i, src.Row(i) - c1);
        }

        Matrix<double> matrix = target.Transpose()* src;
        
        var solved = matrix.Svd(true);
        Matrix<double> V = solved.VT.Transpose();
        Matrix<double> R = V*solved.U.Transpose();
        double det = R.Determinant();

        

        if (det < 0.0){
            V.SetColumn(2, V.Column(2) * -1.0); 
            R = V * solved.U.Transpose();
        }
        Vector<double> tr = c1 - R.Transpose() * c0;
        Debug.Log("offset: " + tr.ToString());
        Quaternion q = Aff3d.RotToQuaternion(R);
        T.Rotation = q;
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
        FilterOutliers(ref ass_idx, 0.1f);

        Matrix<double> tar = target.PositionMatrix(ass_idx, true);
        Matrix<double> src = source.PositionMatrix(ass_idx, false);
        Debug.Log("tar size: " + tar.ToString());
        Debug.Log("src size: " + src.ToString());

        SVDAlign(tar,  src, ref T);
        Debug.Log("MATCH OUTPUT: " + T.ToString());
        source.Transform(T);


        
        return true;

        
        
        // determine correspondence by time

        //use svd to minimize least squares

        //calculate symmetric distance

        //double score = Trajectory::Distance(target, src)



    }
    private double dt_corr_srch_ = 0.1;
}

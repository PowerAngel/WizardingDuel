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

    }
    public virtual bool Match(Trajectory target, Trajectory source, out Aff3d T, out double score)
    {
        T = Aff3d.Identity();
        score = 0;
        return true;
    }
    public void SVDAlign(in Matrix<double> target, ref Matrix<double> src, ref Aff3d T) //target
    {
        
        if(target.RowCount != src.RowCount)
        {
            //ERROR
            return;
        }

        Vector<double> c0 = Vector<double>.Build.Dense(3);
        Vector<double> c1 = Vector<double>.Build.Dense(3);
        for (int i=0;i< target.RowCount; i++){ //calculate centroid
            c0 += target.Row(i);
            c1 += src.Row(i);
        }
        
        c0 *= (1.0 / ((double)target.RowCount)); //normalize
        c1 *= (1.0 / ( (double)target.RowCount));

        Matrix<double> matrix = target.Transpose()* src;

        var solved = matrix.Svd(true);

        Matrix<double> V = solved.VT.Transpose();
        Matrix<double> R = V*solved.U.Transpose();
        double det = R.Determinant();

        double d = target[0, 0];

        if (det < 0.0)
        {
            // nneg++;

            V.SetColumn(2, V.Column(2) * -1.0); 
            R = V * solved.U.Transpose();
        }
        Vector<double> tr = c0 - R.Transpose() * c1;
        
        
        //Vector3 pos = 
        

    }
    private double score_th_success_;

}

public class TICPMatcher : TrajectoryMatcher
{
    public TICPMatcher()
    {
        Debug.Log("constructor matcher");
    }
    void TimeProjectionCorrespondance(Trajectory target, Trajectory source)
    {

    }

    public override bool Match(Trajectory target, Trajectory source, out Aff3d T, out double score)
    {
        
        score = 0;
        T = Aff3d.Identity();
        Debug.Log("matched");
        

        
        return true;

        
        
        // determine correspondence by time

        //use svd to minimize least squares

        //calculate symmetric distance

        //double score = Trajectory::Distance(target, src)



    }
    private double dt_corr_srch_ = 0.1;
}

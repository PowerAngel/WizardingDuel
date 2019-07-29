using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

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
    private double score_th_success_;

}

public class TICPMatcher : TrajectoryMatcher
{
    public TICPMatcher()
    {

    }
    void TimeProjectionCorrespondance(Trajectory target, Trajectory source)
    {

    }

    public override bool Match(Trajectory target, Trajectory source, out Aff3d T, out double score)
    {
        score = 0;
        T = Aff3d.Identity();
        return true;
        // determine correspondence by time

        //use svd to minimize least squares

        //calculate symmetric distance

        //double score = Trajectory::Distance(target, src)



    }
    private double dt_corr_srch_ = 0.1;
}

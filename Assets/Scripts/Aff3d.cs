using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet;
// Check Examples

// Compiler version 4.0, .NET Framework 4.5




/*Gesture recognition.
För att ge en realistisk wizard känsla samt ett bra tempo i combat tror vi att den spell man castar skall bero på hur man rör trollstaven -"swish and flick"

För det krävs att vi kan representera position och orientation på trollstaven samt att korrekt utförda sekvenser kan kännas igen.

Beräkningar med Position och orientation även kallad "pose" (från engelskans posture) kallas i matematiken för affine transformations.För det har vi skapat en typ som heter Affine3d.Affine3d är en väldigt generellt datatyp som kan användas för att göra beräkningar med geometri.T.ex.det första vi gjorde var att importera en trollstav i unity.Målet var att spara ned positionen på toppen på staven till en textfil.Mha.Trackingen av handkontrollen kan vi få positionen och orientation relativt världen och skapa en transformation som vi kallar T_hand.Vi kan också skapa en transformation som bara innehåller stavens längd Twand.
Dessa kan sedan multipliceras för att beställa wandens pose i världen.*/
public class Aff3d
{
    private Quaternion rot;
    private Vector3 trans;
    public Aff3d()
    {
        
        
        

    }
       public Aff3d(float x, float y, float z, float ex, float ey, float ez)
       {
           trans[0] = x;
           trans[1] = y;
           trans[2] = z;

           Vector3 rotationVector = new Vector3(ex, ey, ez);
           rot = Quaternion.Euler(rotationVector);
    }

    public Aff3d(Aff3d aff3d)
    {
        this.trans = aff3d.Translation;
        this.rot = aff3d.Rotation;
    }

    public Aff3d Inverse()
       {

           Aff3d inv = new Aff3d(this);
        //negate translation and use the quaternion inverse operation to invert rotation
        trans = -trans;
        rot = Quaternion.Inverse(rot);
        return inv;
       }

    public Quaternion Rotation
    {
        get { return rot; }
        set { rot = value; }
    }
    
    
    public Vector3 Translation
    {
        get { return trans; }

        set { trans = value; }

    }

    
   
       public override string ToString(){ //format rotation andtranslation to a string and return
        Vector3 eul = rot.eulerAngles.normalized;
        string tmp = trans.x+", "+trans.y+ ","+trans.z+ ","+eul.x+", "+eul.x+", "+eul.z;
        return tmp;
       }



   
   public static Aff3d Identity()
   { // return an aff3d with zeroes trans and rot,  aka Identity trans and rot
       Aff3d ident = new Aff3d(0, 0, 0, 0, 0, 0);
       return ident;
   }

   public static Aff3d operator *(Aff3d T2, Aff3d T1)
   {
       Aff3d T3 = new Aff3d();
       T3.Rotation = T2.Rotation * T1.Rotation;
       T3.Translation = T1.Translation + T1.Rotation * T2.Translation;
       return T3;
   }

public static Quaternion RotToQuaternion(Matrix<double> rot)
    {
        if (rot.RowCount != 3 || rot.ColumnCount != 3)
        {
            Debug.LogError("error, dimensions of rotation incorrect");
        }
        double tr = rot[0, 0] + rot[1, 1] + rot[2, 2];
        double qw, qx, qy, qz;
        if (tr > 0)
        {
            double S = Mathf.Sqrt((float)(tr + 1.0)) * 2; // S=4*qw 
            qw = 0.25 * S;
            qx = (rot[2, 1] - rot[1, 2]) / S;
            qy = (rot[0, 2] - rot[2, 0]) / S;
            qz = (rot[1, 0] - rot[0, 1]) / S;
        }
        else if ((rot[0, 0] > rot[1, 1]) & (rot[0, 0] > rot[2, 2]))
        {
            double S = Mathf.Sqrt((float)(1.0 + rot[0, 0] - rot[1, 1] - rot[2, 2]) ) * 2; // S=4*qx 
            qw = (rot[2, 1] - rot[1, 2]) / S;
            qx = 0.25 * S;
            qy = (rot[0, 1] + rot[1, 0]) / S;
            qz = (rot[0, 2] + rot[2, 0]) / S;
        }
        else if (rot[1, 1] > rot[2, 2])
        {
            double S = Mathf.Sqrt((float)(1.0 + rot[1, 1] - rot[0, 0] - rot[2, 2])) * 2; // S=4*qy
            qw = (rot[0, 2] - rot[2, 0]) / S;
            qx = (rot[0, 1] + rot[1, 0]) / S;
            qy = 0.25 * S;
            qz = (rot[1, 2] + rot[2, 1]) / S;
        }
        else
        {
            
            double S = Mathf.Sqrt( (float)(1.0 + rot[2, 2] - rot[0, 0] - rot[1, 1]) ) * 2; // S=4*qz
            qw = (rot[1, 0] - rot[0, 1]) / S;
            qx = (rot[0, 2] + rot[2, 0]) / S;
            qy = (rot[1, 2] + rot[2, 1]) / S;
            qz = 0.25 * S;
        }
            Quaternion q = new Quaternion((float)qx, (float)qy, (float)qz, (float)qw);
            return q;
    }
   // Add get and set to return rot and translation

   //define * operator for Aff3d and Vector3d
   

}



/*Quaternion är standard för att representera rotation.Har man bara vinklar så kan man konvertera dessa till quaternion format mha.Länk nedan

https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html*/


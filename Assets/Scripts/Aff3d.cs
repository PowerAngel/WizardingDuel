using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

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
        string tmp = "testar!!!";
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


   // Add get and set to return rot and translation

   //define * operator for Aff3d and Vector3d
   

}



/*Quaternion är standard för att representera rotation.Har man bara vinklar så kan man konvertera dessa till quaternion format mha.Länk nedan

https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html*/


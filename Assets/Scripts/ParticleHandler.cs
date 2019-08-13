using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ParticleHandler : MonoBehaviour
{
    public SteamVR_Input_Sources handType; //1
    public SteamVR_Action_Boolean teleportAction; //2
    public SteamVR_Action_Boolean grabAction; //3

    ParticleSystem spell;

    private bool hasGrabed = false;

    // Start is called before the first frame update
    void Start()
    {
        spell = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetGrab())
        {
            hasGrabed = true;
            StartParticleEffect();
        }
        else
        {
            if (hasGrabed)
            {
                print("Spell should have fired");

                hasGrabed = false;
                EndParticleEffect();
            }          
        }
    }

    public bool GetTeleportDown()
    {
        return teleportAction.GetLastStateDown(handType);
    }

    public bool GetGrab()
    {   
        return grabAction.GetState(handType);
    }

    public void StartParticleEffect()
    {
        spell.Play();
    }

    public void EndParticleEffect()
    {
        spell.Stop();
    }
}

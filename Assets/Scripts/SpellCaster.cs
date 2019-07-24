using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SpellCaster : MonoBehaviour
{
    public SteamVR_Input_Sources handType; //1
    public SteamVR_Action_Boolean teleportAction; //2
    public SteamVR_Action_Boolean grabAction; //3

    private bool hasGrabed = false;

    public float fireRate = 2f;
    public float weaponRange = 100f;
    public float hitForce = 500f;
    public Transform wandEnd;

    private WaitForSeconds spellDuration = new WaitForSeconds(0.07f);

    private AudioSource spellAudio;
    private LineRenderer spellLine;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        spellLine = GetComponent<LineRenderer>();
        spellAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetGrab())
        {
            hasGrabed = true;
        }
        else
        {
            if (hasGrabed)
            {
                if(Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;

                    StartCoroutine(SpellEffect());

                    Vector3 rayOrigin = wandEnd.transform.position;

                    RaycastHit hit;

                    spellLine.SetPosition(0, wandEnd.position);

                    print("Spell should have fired");

                    if(Physics.Raycast(rayOrigin, wandEnd.transform.forward, out hit, weaponRange))
                    {
                        spellLine.SetPosition(1, hit.point);
                        if(hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(-hit.normal * hitForce);
                        }
                    }
                    else
                    {
                        spellLine.SetPosition(1, rayOrigin + (wandEnd.transform.forward * weaponRange));
                    }
                }

                hasGrabed = false;
            }
        }
    }

    private IEnumerator SpellEffect()
    {
        spellAudio.Play();
        spellLine.enabled = true;
        yield return spellDuration;

        spellLine.enabled = false;
    }

    public bool GetTeleportDown()
    {
        return teleportAction.GetLastStateDown(handType);
    }

    public bool GetGrab()
    {
        return grabAction.GetState(handType);
    }
}

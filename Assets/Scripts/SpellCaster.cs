using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class SpellCaster : MonoBehaviour
{
    public SteamVR_Input_Sources handType; //1
    public SteamVR_Action_Boolean teleportAction; //2
    public SteamVR_Action_Boolean grabAction; //3

    private bool hasGrabed = false;

    public Material[] myMaterials = new Material[5];

    public float fireRate = 2f;
    public float weaponRange = 100f;
    public float hitForce = 500f;
    public Transform wandEnd;
    public Text spellName;

    private WaitForSeconds spellDuration = new WaitForSeconds(0.07f);
    private WaitForSeconds spellnameDuration = new WaitForSeconds(1.5f);

    private AudioSource spellAudio;
    private LineRenderer spellLine;
    private float nextFire;
    private System.Random random;

    private Spell[] spells = new Spell[5];
    private Spell currentSpell;

    // Start is called before the first frame update
    void Start()
    {
        spellLine = GetComponent<LineRenderer>();
        spellAudio = GetComponent<AudioSource>();

        spells[0] = new Spell("Avada Kedavra", 10, 100, myMaterials[0]);
        spells[1] = new Spell("Stupify", 750, 10, myMaterials[1]);
        spells[2] = new Spell("Sectum Sempra", 100, 66, myMaterials[2]);
        spells[3] = new Spell("Confringo", 5000, 50, myMaterials[3]);
        spells[4] = new Spell("Episkey", 25, -25, myMaterials[4]);
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

                    random = new System.Random();

                    currentSpell = spells[random.Next(0, spells.Length)];

                    //spellLine.material = myMaterials[random.Next(0, myMaterials.Length)];
                    spellLine.material = currentSpell.getMaterial();
                    spellName.text = currentSpell.getName();

                    StartCoroutine(SpellEffect());
                    StartCoroutine(ShowSpellName());

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
                            hit.transform.GetComponent<HealthHandler>().ChangeHP(currentSpell.getDamage());
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

    private IEnumerator ShowSpellName()
    {
        yield return spellnameDuration;
        spellName.text = "";

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    private int HP = 100;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHP(int damage = 25)
    {
        HP -= damage;

        if(HP < 0)
        {
            HP = 0;
        }

        text.text = HP + "%";
    }
}

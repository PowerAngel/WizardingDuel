using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    private int HP = 100;
    public Text text;
    public Image bar;
    private GameObject gameCoordinator;

    // Start is called before the first frame update
    void Start()
    {
        gameCoordinator = GameObject.Find("GameCoordinator");
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

        bar.fillAmount = (float)HP / 100;

        if(HP == 0)
        {
            gameCoordinator.GetComponent<GameCoordinator>().RemoveGameObject(gameObject);
            gameCoordinator.GetComponent<GameCoordinator>().InstantiateGameObject();
        }
    }
}

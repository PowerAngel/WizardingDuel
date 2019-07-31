using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : MonoBehaviour
{
    public GameObject myPrefab;
    //private GameObject[] Targets;
    List<GameObject> Targets;
    // Start is called before the first frame update
    void Start()
    {
        Targets = new List<GameObject>();
        GameObject tempObject = Instantiate(myPrefab, new Vector3(-6.25f, 1.25f, -7), Quaternion.identity);

        Vector3 direction = tempObject.transform.position - Camera.main.transform.position;
        tempObject.transform.rotation = Quaternion.LookRotation(direction);

        Targets.Add(tempObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveGameObject(GameObject gameObject)
    {
        Targets.Remove(gameObject);
        Destroy(gameObject);
    }

    public void InstantiateGameObject()
    {
        float x = Random.Range(-4f, -8f);
        float y = Random.Range(1f, 1.5f);
        float z = Random.Range(-5f, -8f);

        GameObject tempObject = Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
        Vector3 direction = tempObject.transform.position - Camera.main.transform.position;
        tempObject.transform.rotation = Quaternion.LookRotation(direction);
        Targets.Add(tempObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldGeneratorScript : MonoBehaviour
{
    public GameObject demoBuilding;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("running");
        for (int i = 0; i < 10; i+=4)
        {
            for (int j = 0; j < 10; j+=2)
            {
                Instantiate(demoBuilding, new Vector3(0 + i, 2, 0 + j), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

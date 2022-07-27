using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldGeneratorScript : MonoBehaviour
{
    public GameObject demoBuilding;
    public GameObject demoRoadTile;
    public GameObject demoShop;
    public GameObject demoHouse;

    public int[,] mapArray =
    {
        { 1, 0, 1, 2, 3 }, 
        { 3, 0, 2, 1, 2 }, 
        { 2, 0, 1, 3, 0 }, 
        { 1, 0, 3, 1, 0 },
        { 3, 0, 0, 0, 0 },
        { 1, 2, 1, 3, 1 }
    };

    private int tileSize = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(mapArray.GetLength(0));
        Debug.Log(mapArray.GetLength(1));
        for (int col = 0; col < mapArray.GetLength(0); col++)
        {
            for (int row = 0; row < mapArray.GetLength(1); row++)
            {
                if (mapArray[col, row] == 0)
                {
                    Instantiate(demoRoadTile, new Vector3(0 + (col*tileSize), 2, 0 + (row*tileSize)), Quaternion.identity);
                }
                if (mapArray[col, row] == 1)
                {
                    Instantiate(demoBuilding, new Vector3(0 + (col*tileSize), 2, 0 + (row * tileSize)), Quaternion.identity);
                }
                if (mapArray[col, row] == 2)
                {
                    Instantiate(demoShop, new Vector3(0 + (col*tileSize), 2, 0 + (row * tileSize)), Quaternion.identity);
                }
                if (mapArray[col, row] == 3)
                {
                    Instantiate(demoHouse, new Vector3(0 + (col*tileSize), 2, 0 + (row * tileSize)), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

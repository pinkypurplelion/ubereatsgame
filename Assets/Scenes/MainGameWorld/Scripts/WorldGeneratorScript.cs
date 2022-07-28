using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes.MainGameWorld.Scripts;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WorldGeneratorScript : MonoBehaviour
{
    public GameObject demoBuilding;
    public GameObject demoRoadTile;
    public GameObject demoShop;
    public GameObject demoHouse;

    private int tileSize = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        CityBlock block = new CityBlock {BlockDimension = 8, Connections = new []{2, 6, 4, 3}};
        block.CreateMap();

        foreach (var tile in block.Tiles)
        {
            if (tile.Type == 0)
            {
                Instantiate(demoRoadTile, new Vector3(0 + (tile.X*tileSize), 2, 0 + (tile.Y*tileSize)), Quaternion.identity);
            }
            if (tile.Type == 1)
            {
                Instantiate(demoBuilding, new Vector3(0 + (tile.X*tileSize), 2, 0 + (tile.Y*tileSize)), Quaternion.identity);
            }
        }
        
        // int[,] mapArray = GenerateCityBlock(8, 2, 3, 6, 1);
        //
        //
        // for (int col = 0; col < mapArray.GetLength(0); col++)
        // {
        //     for (int row = 0; row < mapArray.GetLength(1); row++)
        //     {
        //         if (mapArray[col, row] == 1)
        //         {
        //         }
        //         if (mapArray[col, row] == 2)
        //         {
        //             Instantiate(demoBuilding, new Vector3(0 + (col*tileSize), 2, 0 + (row * tileSize)), Quaternion.identity);
        //         }
        //         // if (mapArray[col, row] == 2)
        //         // {
        //         //     Instantiate(demoShop, new Vector3(0 + (col*tileSize), 2, 0 + (row * tileSize)), Quaternion.identity);
        //         // }
        //         if (mapArray[col, row] == 3)
        //         {
        //             Instantiate(demoHouse, new Vector3(0 + (col*tileSize), 2, 0 + (row * tileSize)), Quaternion.identity);
        //         }
        //     }
        // }
    }


    // 0 = nothing, 1 road, 2 building
    int[,] GenerateCityBlock(int dimension, int north, int east, int south, int west)
    {
        int[,] cityBlock = new int[dimension, dimension];
        cityBlock[north, 0] = 3;
        cityBlock[dimension - 1, east] = 3;
        cityBlock[south, dimension - 1] = 3;
        cityBlock[0, west] = 3;

        foreach (var point in FindPath(new []{north, 0}, new []{south, dimension - 1}))
        {
            cityBlock[point[0], point[1]] = 1;
        }
        foreach (var point in FindPath(new []{north, 0}, new []{0, west}))
        {
            cityBlock[point[0], point[1]] = 1;
        }
        foreach (var point in FindPath(new []{north, 0}, new []{dimension - 1, east}))
        {
            cityBlock[point[0], point[1]] = 1;
        }
        Debug.Log(cityBlock.ToString());
        return cityBlock;
    }

    List<int[]> FindPath(int[] pointA, int[] pointB)
    {
        List<int[]> path = new List<int[]>();
        int[] newX = pointA;
        if (pointA[0] != pointB[0])
        {
            var x = pointA[0] - pointB[0];
            for (var i = 0; i < Math.Abs(x); i++)
            {
                newX = x < 0 ? new[] { pointA[0] + i, pointA[1] } : new[] { pointA[0] - i, pointA[1] };
                path.Add(newX);
            }
        }
        if (pointA[1] != pointB[1])
        {
            var y = pointA[1] - pointB[1];
            for (var i = 0; i < Math.Abs(y); i++)
            {
                path.Add(y < 0 ? new[] { newX[0], pointA[1] + i } : new[] { newX[0], pointA[1] - i });
            }
        }
        Debug.Log(path.ToString());
        return path;
    }
    
    
    
    
    
    
    
    

    
    
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

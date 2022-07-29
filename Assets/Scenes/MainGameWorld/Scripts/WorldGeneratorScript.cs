using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes.MainGameWorld.Scripts;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

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
        int WorldDimension = 2;
        int BlockDimension = 8;
        
        WorldMap map = new WorldMap {WorldDimension = WorldDimension, BlockDimension = BlockDimension};
        map.GenerateWorld();

        foreach (var block in map.CityBlocks)
        {
            foreach (var tile in block.Tiles)
            {
                if (tile.Type == 0)
                {
                    Instantiate(demoRoadTile, new Vector3(
                        block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                        2, 
                        block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                }
                if (tile.Type == 1)
                {
                    Instantiate(demoBuilding, new Vector3(
                        block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                        2, 
                        block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                }
                if (tile.Type == 2)
                {
                    Instantiate(demoHouse, new Vector3(
                        block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                        2, 
                        block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                }
            }
        }
        
        
        // CityBlock block = new CityBlock {BlockDimension = 8, Connections = new []{2, 6, 3, 5}};
        // block.CreateMap();


    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

using Scenes.MainGameWorld.Scripts;
using UnityEngine;

public class WorldGeneratorScript : MonoBehaviour
{
    public GameObject demoBuilding;
    public GameObject demoRoadTile;
    public GameObject demoShop;
    public GameObject demoHouse;

    public GameObject TilePrefab;

    private int tileSize = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        int WorldDimension = 1;
        int BlockDimension = 8;
        
        // Generates the World based on the WorldDimension and BlockDimension
        WorldMap map = new WorldMap {WorldDimension = WorldDimension, BlockDimension = BlockDimension};
        map.GenerateWorld();

        // Draws the World based on the WorldMap graph.
        foreach (var block in map.CityBlocks)
        {
            foreach (var tile in block.Tiles)
            {
                GameObject t = Instantiate(TilePrefab, new Vector3(
                    block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                    2, 
                    block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                t.GetComponent<TileObject>().Tile = tile;
                
                // if (tile.Type == TileType.Road) // Tile type == 0 => road tile
                // {
                //     Instantiate(demoRoadTile, new Vector3(
                //         block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                //         2, 
                //         block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                // }
                // if (tile.Type == TileType.Building && tile.NextToRoad) // Tile types == 1 => building tile
                // {
                //     Instantiate(demoBuilding, new Vector3(
                //         block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                //         2, 
                //         block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                // }
                // if (tile.Type == TileType.Shop) // Tile types == 1 => building tile
                // {
                //     Instantiate(demoShop, new Vector3(
                //         block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                //         2, 
                //         block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                // }
                // if (tile.Type == TileType.House) // Tile types == 1 => building tile
                // {
                //     Instantiate(demoHouse, new Vector3(
                //         block.BlockX * BlockDimension * tileSize + (tile.X*tileSize), 
                //         2, 
                //         block.BlockY * BlockDimension * tileSize + (tile.Y*tileSize)), Quaternion.identity);
                // }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

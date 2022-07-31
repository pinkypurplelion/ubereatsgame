using Scenes.MainGameWorld.Scripts;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject tilePrefab;

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
                GameObject t = Instantiate(tilePrefab, new Vector3(
                    block.BlockX * BlockDimension * tileSize + tile.X*tileSize, 
                    2, 
                    block.BlockY * BlockDimension * tileSize + tile.Y*tileSize), Quaternion.identity);
                t.GetComponent<TileObject>().Tile = tile;
            }
        }
    }
}

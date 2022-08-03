using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class RoadBlock : MonoBehaviour
    {
        /**
         * This script determines the orientation of the road based of its location
         * As well as the type of road piece to use
        **/
        public Tile Tile;
        public TileType TileType;
        public List<Tile> Tiles { get; } = new();

        // Start is called before the first frame update
        void Start()
        {
            
            foreach (var tile in Tiles)
            {
                if (tile.Type == TileType.Road)
                {
                    int[,] directions = new int[4,2];
                    int count = 0;
                    foreach (var link in tile.Connections)
                    {
                        
                        
                        if (link.ConnectedTile.Type == TileType.Road)
                        {

                            directions[count, 0] = link.ConnectedTile.X;
                            directions[count, 1] = link.ConnectedTile.Y;
                            
                        }
                    }
                }
                
                
            }

        }

        
    }
}

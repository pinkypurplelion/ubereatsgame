using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// The script attached to the TileObject prefab. This currently only manages the models but will manage all
    /// game functionality of the prefab in relation to the information provided by the attached Tile script.
    /// Tile script attached at runtime by the WorldEventManager script.
    /// </summary>
    public class TileObject : MonoBehaviour
    {
        public Tile Tile;
        public TileType TileType;
        private GameObject _road;

        private void Start()
        {
            TileType = Tile.Type;

            switch (Tile.Type)
            {
                case TileType.Road:
                    transform.Find("road").gameObject.SetActive(true);
                    _road = transform.Find("road").gameObject;
                    _road.SetActive(true);
                    _road.GetComponent<RoadTile>().Tile = Tile;
                    break;
                case TileType.Building:
                    transform.Find("building").gameObject.SetActive(true);
                    RotateTile();
                    break;
                case TileType.Shop:
                    transform.Find("shop").gameObject.SetActive(true);
                    RotateTile();
                    break;
                case TileType.House:
                    transform.Find("house").gameObject.SetActive(true);
                    RotateTile();
                    break;
                case TileType.Landscape:
                    transform.Find("landscape").gameObject.SetActive(true);
                    break;
            }

            
        }

        void RotateTile()
        {
            var connected = Tile.Connections.Find(connection => connection.ConnectedTile.Type == TileType.Road);
            var x = connected.ConnectedTile.X;
            var y = connected.ConnectedTile.Y;
            if (Tile.X == x && Tile.Y > y)
            {
                transform.Rotate(0, 0, 0);
            } 
            else if (Tile.X == x && Tile.Y < y)
            {
                transform.Rotate(0, 180, 0);
            }
            else if (Tile.X > x && Tile.Y == y)
            {
                transform.Rotate(0, 90, 0);
            }
            else if (Tile.X < x && Tile.Y == y)
            {
                transform.Rotate(0, 270, 0);
            }
        }
    }
}
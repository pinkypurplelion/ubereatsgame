using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.MainGameWorld.Scripts
{
    /**
     * The script attached to the TileObject prefab. This currently only manages the models but will manage all
     * game functionality of the prefab in relation to the information provided by the attached Tile script.
     *
     * Tile script attached at runtime by the WorldGenerator script.
     */
    public class TileObject : MonoBehaviour
    {
        public Tile Tile;
        public TileType TileType;

        void Start()
        {
            TileType = Tile.Type;
            
            switch (Tile.Type)
            {
                case TileType.Road:
                    transform.Find("road").gameObject.SetActive(true);
                    break;
                case TileType.Building:
                    transform.Find("building").gameObject.SetActive(true);
                    break;
                case TileType.Shop:
                    transform.Find("shop").gameObject.SetActive(true);
                    break;
                case TileType.House:
                    transform.Find("house").gameObject.SetActive(true);
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.MainGameWorld.Scripts
{
    public class TileObject : MonoBehaviour
    {
        public Tile Tile;

        void Start()
        {
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
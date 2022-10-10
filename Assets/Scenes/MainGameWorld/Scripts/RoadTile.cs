using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// This script determines the orientation of the road based of its location in the world and
    /// the type of road piece to use.
    /// </summary>
    /// <author>Jayath Gunawardena, modified by Liam Angus</author>
    public class RoadTile : MonoBehaviour
    {
        public Tile Tile;

        private readonly Dictionary<string, bool> _directions = new();
        
        private void Start()
        {
            foreach (var connection in Tile.Connections.Where(connection => connection.ConnectedTile.Type == TileType.Road))
            {
                if (connection.ConnectedTile.X == Tile.X)
                {
                    _directions.Add(connection.ConnectedTile.Y == Tile.Y + 1 ? "north" : "south", true);
                }

                else
                {
                    _directions.Add(connection.ConnectedTile.X == Tile.X + 1 ? "east" : "west", true);
                }
            }

            switch (_directions.Count)
            {
                case 0:
                    break;
                case 1:
                    //Return the straight block
                    transform.Find("RoadObject").gameObject.SetActive(true);
                    transform.Find("4wayIntersection").gameObject.SetActive(false);
                    transform.Find("3wayIntersection").gameObject.SetActive(false);
                    transform.Find("curvedRoad2").gameObject.SetActive(false);
                    if (_directions.ContainsKey("north") | _directions.ContainsKey("south"))
                    {
                        transform.Find("RoadObject").Rotate(0, 90f, 0, Space.Self);
                    }
                    break;
                case 2:
                    //Return either of the 2 way blocks
                    transform.Find("4wayIntersection").gameObject.SetActive(false);
                    transform.Find("3wayIntersection").gameObject.SetActive(false);
                    transform.Find("curvedRoad2").gameObject.SetActive(false);
                    transform.Find("RoadObject").gameObject.SetActive(false);
                    if (_directions.ContainsKey("north"))
                    {
                        if (_directions.ContainsKey("south"))
                        {
                            transform.Find("RoadObject").gameObject.SetActive(true);
                            transform.Find("RoadObject").Rotate(0, 90f, 0, Space.Self);
                            break;
                        }

                        if (_directions.ContainsKey("east")) {
                            transform.Find("curvedRoad2").gameObject.SetActive(true);
                            transform.Find("curvedRoad2").Rotate(0, 270f, 0, Space.Self);
                            break;
                        }

                        transform.Find("curvedRoad2").gameObject.SetActive(true);
                        transform.Find("curvedRoad2").Rotate(0, 180f, 0, Space.Self);
                    }
                    else if (_directions.ContainsKey("east"))
                    {
                        if (_directions.ContainsKey("west"))
                        {
                            transform.Find("RoadObject").gameObject.SetActive(true);
                        }
                        else
                        {
                            transform.Find("curvedRoad2").gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        transform.Find("curvedRoad2").gameObject.SetActive(true);
                        transform.Find("curvedRoad2").Rotate(0, 90f, 0, Space.Self);
                    }
                    break;
                case 3:
                    //Return the 3 way block
                    transform.Find("curvedRoad2").gameObject.SetActive(false);
                    transform.Find("4wayIntersection").gameObject.SetActive(false);
                    transform.Find("RoadObject").gameObject.SetActive(false);
                    if (_directions.ContainsKey("north"))
                    {
                        if (_directions.ContainsKey("south"))
                        {
                            if (_directions.ContainsKey("east"))
                            {
                                transform.Find("3wayIntersection").gameObject.SetActive(true);
                                transform.Find("3wayIntersection").Rotate(0f, 270.0f, 0.0f, Space.Self);
                            }
                            else
                            {
                                transform.Find("3wayIntersection").gameObject.SetActive(true);
                                transform.Find("3wayIntersection").Rotate(0f, 90.0f, 0.0f, Space.Self);
                            }
                        }
                        else
                        {
                            transform.Find("3wayIntersection").gameObject.SetActive(true);
                            transform.Find("3wayIntersection").Rotate(0f, 180.0f, 0.0f, Space.Self);
                        }
                    }
                    else
                    {
                        transform.Find("3wayIntersection").gameObject.SetActive(true);

                    }
                    break;
                case 4:
                    //Return the 4 way intersection block
                    transform.Find("curvedRoad2").gameObject.SetActive(false);
                    transform.Find("3wayIntersection").gameObject.SetActive(false);
                    transform.Find("RoadObject").gameObject.SetActive(false);
                    transform.Find("4wayIntersection").gameObject.SetActive(true);
                    break;
            }
        }


    }
}

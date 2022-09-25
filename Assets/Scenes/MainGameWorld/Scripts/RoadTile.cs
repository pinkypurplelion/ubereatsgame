using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class RoadTile : MonoBehaviour
    {
        /**
         * This script determines the orientation of the road based of its location
         * As well as the type of road piece to use
        **/
        public Tile Tile;
        public TileType TileType;
        public CityBlock CityBlock;
        public Dictionary<string, bool> directions = new Dictionary<string, bool>();
        // Start is called before the first frame update
        void Start()
        {

            foreach (var connection in Tile.Connections)
            {
                if(connection.ConnectedTile.Type == TileType.Road)
                {
                    if (connection.ConnectedTile.X == Tile.X)
                    {
                        if (connection.ConnectedTile.Y == Tile.Y + 1)
                        {
                            directions.Add("north", true);
                        }
                        else { directions.Add("south", true); }
                    }

                    else
                    {
                        if (connection.ConnectedTile.X == Tile.X + 1)
                        { directions.Add("east", true); }
                        else { directions.Add("west", true); }

                    }
                }
           
          
            }

            switch (directions.Count)
            {
                case 0:
                    
                    break;
                case 1:
                    //Return the straight block
                    transform.Find("RoadObject").gameObject.SetActive(true);
                    transform.Find("4wayIntersection").gameObject.SetActive(false);
                    transform.Find("3wayIntersection").gameObject.SetActive(false);
                    transform.Find("curvedRoad2").gameObject.SetActive(false);
                    if (directions.ContainsKey("north") | directions.ContainsKey("south"))
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
                    if (directions.ContainsKey("north"))
                    {
                        if (directions.ContainsKey("south"))
                        {
                            transform.Find("RoadObject").gameObject.SetActive(true);
                            transform.Find("RoadObject").Rotate(0, 90f, 0, Space.Self);
                            break;
                        }
                        else if (directions.ContainsKey("east")) {
                            transform.Find("curvedRoad2").gameObject.SetActive(true);
                            transform.Find("curvedRoad2").Rotate(0, 270f, 0, Space.Self);
                            break;
                        }
                        else {
                            transform.Find("curvedRoad2").gameObject.SetActive(true);
                            transform.Find("curvedRoad2").Rotate(0, 180f, 0, Space.Self);
                            break;
                        }


                    }else if (directions.ContainsKey("east"))
                    {
                        if (directions.ContainsKey("west"))
                        {
                            transform.Find("RoadObject").gameObject.SetActive(true);
                            break;

                        }
                        else
                        {
                            transform.Find("curvedRoad2").gameObject.SetActive(true);
                            break;
                        }
                    }
                    else
                    {
                        transform.Find("curvedRoad2").gameObject.SetActive(true);
                        transform.Find("curvedRoad2").Rotate(0, 90f, 0, Space.Self);
                        break;
                    }

                    break;
                case 3:
                    //Return the 3 way block
                    transform.Find("curvedRoad2").gameObject.SetActive(false);
                    transform.Find("4wayIntersection").gameObject.SetActive(false);
                    transform.Find("RoadObject").gameObject.SetActive(false);
                    if (directions.ContainsKey("north"))
                    {
                        if (directions.ContainsKey("south"))
                        {


                            if (directions.ContainsKey("east"))
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
                default:
                    break;

            }

        }


    }
}

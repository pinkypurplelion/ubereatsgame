using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public List<Guid> Orders { get; set; } = new();
        // List to future proof in case two shops next to each other. TODO: implement way to select between orders/shops to pick up
        public List<Collider> CurrentShopCollisions { get; set; } = new();
        
        //List to future proof in case two houses next to each other. TODO: implement way to select between orders/houses to drop off
        public List<Collider> CurrentHouseCollisions { get; set; } = new();

        
        public float speed;

        public int orderCount;
        
        private Rigidbody _rigidbody;

        private BoxCollider _collider;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
        }

        public Vector2 moveVal;

        // Called based on Movement action
        void OnMovement(InputValue value)
        {
            moveVal = value.Get<Vector2>();
        }


        
        // Update is called once per frame
        void Update()
        {
            Vector3 tempVect = new Vector3(moveVal.x, 0, moveVal.y);
            tempVect = tempVect.normalized * (speed * Time.deltaTime);
            _rigidbody.MovePosition(transform.position + tempVect);
        }
        
        

        
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided with " + other.name);
            if (other.CompareTag("Shop") && !CurrentShopCollisions.Contains(other))
            {
                CurrentShopCollisions.Add(other);
            }
            else if (other.CompareTag("House") && !CurrentHouseCollisions.Contains(other))
            {
                CurrentHouseCollisions.Add(other);
            }
            Debug.Log("CurrentShopCollisions: " + CurrentShopCollisions.Count);
            Debug.Log("CurrentHouseCollisions: " + CurrentHouseCollisions.Count);
            //Check collider for specific properties (Such as tag=item or has component=item)
        }

        void OnPickUpDropOff(InputValue value)
        {
            foreach (var collision in CurrentShopCollisions)
            {
                ShopTile tile = collision.transform.GetComponent<ShopTile>();
                if (tile.Orders.Count > 0)
                {
                    PickupOrder(tile);
                }
            }
            
            if (CurrentHouseCollisions.Count > 0)
            {
                DeliverOrder(CurrentHouseCollisions[0].transform.GetComponent<HouseTile>());
            }
            Debug.Log("Pick up/drop off action");
        }
        
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Left collider with " + other.name);
            CurrentShopCollisions.Remove(other);
            CurrentHouseCollisions.Remove(other);
            Debug.Log("CurrentShopCollisions: " + CurrentShopCollisions.Count);
        }

        /**
         * This is called when the player is in range of a shop and presses the interact button.
         */
        void PickupOrder(ShopTile tile)
        {
            Debug.Log("Picking up order");
            Orders.Add(tile.Orders[0]);
            tile.Orders.RemoveAt(0);
            orderCount = Orders.Count;
        }
        
        
        /**
         * This is called when the player is in range of a house and presses the interact button.
         */
        void DeliverOrder(HouseTile tile)
        {
            Debug.Log("Delivering order");
            Orders.RemoveAt(0);
            orderCount = Orders.Count;
        }
    }
}

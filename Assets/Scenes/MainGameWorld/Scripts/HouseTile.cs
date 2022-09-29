using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class HouseTile : MonoBehaviour
    {
        public List<Customer> Customers = new();

        public Guid HouseID = Guid.NewGuid();

        public List<Guid> DeliveredOrders = new();
        public bool isDelivering;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isDelivering)
            {
                // _orderText.text = "DELIVERING ORDER";
            }
        }
    }
}

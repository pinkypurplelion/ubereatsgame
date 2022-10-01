using System;
using UnityEditor;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class Order
    {
        public Guid OrderID = Guid.NewGuid();
        public bool Delivered = false;
        public float CreationTime { get; set; }
        public float PickupTime { get; set; }
        public float TimeToDeliver { get; set; }

        public Customer Customer;

        public float OrderValue {get; set;}
        
        public Guid HouseID {get; set;}
        public Guid ShopID {get; set;}
    }
}
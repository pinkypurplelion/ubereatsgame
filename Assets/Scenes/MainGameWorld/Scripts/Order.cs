using System;
using UnityEditor;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class Order
    {
        public Guid OrderID = Guid.NewGuid();
        public float CreationTime = Time.fixedTime;
        public bool Delivered = false;

        public Customer Customer;

        public float OrderValue {get; set;}
        
        public Guid HouseID {get; set;}
        public Guid ShopID {get; set;}
    }
}
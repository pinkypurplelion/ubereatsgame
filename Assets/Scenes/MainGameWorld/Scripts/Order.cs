using System;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class Order
    {
        public Guid OrderID = Guid.NewGuid();
        public float CreationTime = Time.fixedTime;
        
        public float OrderPrice {get; set;}
    }
}
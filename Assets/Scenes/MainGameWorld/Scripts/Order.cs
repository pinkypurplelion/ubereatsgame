using System;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Holds information about orders
    /// </summary>
    /// <author>Liam Angus</author>
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
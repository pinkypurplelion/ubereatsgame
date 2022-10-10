using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Responsible for managing visual elements of the HouseTiles to identify order delivery locations
    /// </summary>
    /// <author>Liam Angus</author>
    public class HouseTile : MonoBehaviour
    {
        public readonly List<Customer> Customers = new();

        public Guid HouseID = Guid.NewGuid();

        public List<Guid> DeliveredOrders = new();
        public bool isDelivering;
        
        private TMP_Text _priceText;
        
        /// <summary>
        /// Called when the tiles are initialised into the world
        /// </summary>
        void Awake()
        {
            _priceText = transform.Find("Canvas").Find("text").GetComponent<TMP_Text>();
            _priceText.color = Color.white;
        }

        /// <summary>
        /// Called once per physics update
        /// </summary>
        void FixedUpdate()
        {
            // Will show an 'X' on the minimap if there is a customer waiting for an order
            _priceText.text = isDelivering ? "X" : "";
        }
    }
}

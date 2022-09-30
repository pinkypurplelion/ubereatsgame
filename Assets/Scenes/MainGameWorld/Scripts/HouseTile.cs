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
        
        private TMP_Text _priceText;
        
        void Awake()
        {
            _priceText = transform.Find("Canvas").Find("text").GetComponent<TMP_Text>();
            _priceText.color = Color.white;
        }

        void Start()
        {
            // purely for testing

        }
        

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isDelivering)
            {
                _priceText.text = "X";
                // _orderText.text = "DELIVERING ORDER";
            }
            else
            {
                _priceText.text = "";
            }
        }
    }
}

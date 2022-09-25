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
    
        private TMP_Text _orderText;

        private void Awake()
        {
            _orderText = transform.Find("text").GetComponent<TMP_Text>();
            _orderText.color = Color.white;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Time.fixedTime % 1f == 0 && !isDelivering)
            {
                _orderText.text = $"Delivered Orders: {DeliveredOrders.Count.ToString()}";
            }

            if (isDelivering)
            {
                _orderText.text = "DELIVERING ORDER";
            }
        }
    }
}

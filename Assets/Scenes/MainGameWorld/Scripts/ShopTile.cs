using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class ShopTile : MonoBehaviour
    {
        public string Name { get; set; }
        public Guid ShopID = Guid.NewGuid();
        public List<Guid> Orders { get; set; } = new();

        private TMP_Text _priceText;

        
        
        void Awake()
        {
            _priceText = transform.Find("text").GetComponent<TMP_Text>();
            _priceText.color = Color.white;
        }

        void Start()
        {

        }

        private void FixedUpdate()
        {
            // Update every second
            if (Time.fixedTime % 1f == 0)
            {
                _priceText.text = Orders.Count.ToString();
            }
        }

        void Update()
        {
            
        }
    }
}
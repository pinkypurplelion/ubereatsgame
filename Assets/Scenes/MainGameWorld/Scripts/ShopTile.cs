using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class ShopTile : MonoBehaviour
    {
        public string Name { get; set; }
        public Guid ShopID = Guid.NewGuid();
        public List<Guid> Orders { get; set; } = new();

        private TMP_Text _priceText;

        public Tile tile;
        
        private void Awake()
        {
            _priceText = transform.Find("Canvas").Find("text").GetComponent<TMP_Text>();
            _priceText.color = Color.white;
            
            int shop = Random.Range(0, 3);
            transform.Find($"shop{shop}").gameObject.SetActive(true);
        }
        
        private void FixedUpdate()
        {
            // Update every second
            if (Time.fixedTime % 1f == 0)
            {
                _priceText.text = $"{Orders.Count.ToString()}";
            }
        }
    }
}
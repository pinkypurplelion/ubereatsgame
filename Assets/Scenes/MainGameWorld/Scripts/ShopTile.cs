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
        

        
        
        void Awake()
        {

        }

        void Start()
        {
        }

        private void FixedUpdate()
        {
            // Update every second

        }

        void Update()
        {
            
        }
    }
}
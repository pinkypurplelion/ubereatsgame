using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerVehicle: MonoBehaviour
    {
        void Start()
        {
            transform.Find("Car0").gameObject.SetActive(true);
        }

        void Update()
        {
            
        }

        void NewVehicle(String vehicleChoice, String currentVehicle)
        {
            transform.Find(currentVehicle).gameObject.SetActive(false);
            transform.Find(vehicleChoice).gameObject.SetActive(true);
        }
    }
}
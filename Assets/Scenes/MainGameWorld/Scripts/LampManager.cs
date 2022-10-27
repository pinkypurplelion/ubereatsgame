using System;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class LampManager : MonoBehaviour
    {
        public GameObject[] lamps;
        public bool isDay = false;
        
        private GameObject _worldEventManagerGameObject;
        private WorldEventManager _worldEventManager;

        private void Awake()
        {
            // Gets the WorldEventManager Object
            _worldEventManagerGameObject = GameObject.Find("WorldEventManager");
            _worldEventManager = _worldEventManagerGameObject.GetComponent<WorldEventManager>();
        }

        private void Update()
        {
            if (_worldEventManager.currentTime % 360 > 60 && !isDay)
            {
                isDay = true;
                foreach (var lamp in lamps)
                {
                    lamp.SetActive(false);
                }
            }
            else if (_worldEventManager.currentTime > 270 && isDay)
            {
                isDay = false;
                foreach (var lamp in lamps)
                {
                    lamp.SetActive(true);
                }
            }
        }
    }
}
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class ThrownOrder : MonoBehaviour
    {
        private float _startTime;

        void Start()
        {
            _startTime = Time.time;
        }

        void Update()
        {
            if (Time.time - _startTime > 3000.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
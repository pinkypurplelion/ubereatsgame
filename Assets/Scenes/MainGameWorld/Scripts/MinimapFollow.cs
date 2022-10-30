using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Used for the minimap camera to smoothly follow the player.
    /// </summary>
    /// <author>Liam Angus</author>
    public class MinimapFollow : MonoBehaviour
    {
        public Transform target;

        public float smoothSpeed = 0.125f;
        public Vector3 offset;

        void FixedUpdate()
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
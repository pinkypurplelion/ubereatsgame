using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Used to control the camera that follows the player.
    /// The script is attached to the camera, and the player object is found dynamically. 
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;

        public float smoothSpeed = 0.125f;
        public Vector3 offset;
        private bool _istargetNull;

        /// <summary>
        /// Used to find the player object.
        /// </summary>
        private void Start()
        {
            _istargetNull = target == null;
        }

        /// <summary>
        /// Called every physics update. Used to follow the player by updating the camera transform position and rotation.
        /// </summary>
        void FixedUpdate()
        {
            if (_istargetNull)
            {
                // Finds the player object for the camera to follow
                target = GameObject.FindGameObjectWithTag("Player").transform;
                _istargetNull = false;
            }
            
            // Used to move the camera to follow the player smoothly
            float currentAngle = transform.eulerAngles.y;
            float desiredAngle = target.transform.eulerAngles.y;
            float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * smoothSpeed);

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            var transform1 = target.transform;
            transform.position = transform1.position - (rotation * offset);

            transform.LookAt(transform1);
        }
    }
}
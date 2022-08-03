using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public List<Guid> Orders { get; set; } = new();

        public float speed;
        
        private Rigidbody _rigidbody;

        private BoxCollider _collider;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
        }

        public Vector2 moveVal;

        // Called based on Movement action
        void OnMovement(InputValue value)
        {
            moveVal = value.Get<Vector2>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 tempVect = new Vector3(moveVal.x, 0, moveVal.y);
            tempVect = tempVect.normalized * (speed * Time.deltaTime);
            _rigidbody.MovePosition(transform.position + tempVect);
        }
        
        
        /**
         * This is called when the player is in range of a shop and presses the interact button.
         */
        void PickupOrder()
        {
            
        }
        
        
        /**
         * This is called when the player is in range of a house and presses the interact button.
         */
        void DeliverOrder()
        {
            
        }
        
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided with " + other.name);
            //Check collider for specific properties (Such as tag=item or has component=item)
        }
    }
}

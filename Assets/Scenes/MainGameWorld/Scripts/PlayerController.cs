using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes.MainGameWorld.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        
        private Rigidbody rigidbody;
        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public Vector2 moveVal;

        // Callled based on Movement action
        void OnMovement(InputValue value)
        {
            moveVal = value.Get<Vector2>();
            Debug.Log(moveVal);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 tempVect = new Vector3(moveVal.x, 0, moveVal.y);
            tempVect = tempVect.normalized * (speed * Time.deltaTime);
            rigidbody.MovePosition(transform.position + tempVect);
        }
    }
}

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

		/// <summary>
		/// Called every physics update. Used to follow the player by updating the camera transform position and rotation.
		/// </summary>
		void FixedUpdate ()
		{
			if (target == null)
			{
				target = GameObject.FindGameObjectWithTag("Player").transform;
			}

			float currentAngle = transform.eulerAngles.y;
			float desiredAngle = target.transform.eulerAngles.y;
			float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * smoothSpeed);
         
			Quaternion rotation = Quaternion.Euler(0, angle, 0);
			transform.position = target.transform.position - (rotation * offset);
         
			transform.LookAt(target.transform);
		}
	}
}

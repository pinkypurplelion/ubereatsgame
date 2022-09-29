using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;

	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	void FixedUpdate ()
	{
		if (target == null)
		{
			target = GameObject.FindGameObjectWithTag("Player").transform;
		}
		//
		// float desiredAngle = target.transform.eulerAngles.y;
		// Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);		
		//
		//
		//
		// Vector3 desiredPosition = target.position + Vector3.Scale(target.forward,offset) - rotation * offset;
		// Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		// transform.position = smoothedPosition;
		//
		// transform.LookAt(target);
		
		
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * smoothSpeed);
         
		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = target.transform.position - (rotation * offset);
         
		transform.LookAt(target.transform);
	}
}

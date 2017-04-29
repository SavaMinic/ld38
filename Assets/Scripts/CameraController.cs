﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	private Camera myCamera;

	public Transform targetCharacter;

	public float speed;
	public float friction;
	public float lerpSpeed;

	private float xDeg;
	private float yDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;

	void Start()
	{
		myCamera = GetComponent<Camera>();	
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			xDeg -= Input.GetAxis("Mouse X") * speed * friction;
			yDeg += Input.GetAxis("Mouse Y") * speed * friction;
			fromRotation = transform.rotation;
			toRotation = Quaternion.Euler(yDeg, xDeg, 0);
			transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);

			// reset z
			var angles = transform.rotation.eulerAngles;
			angles.z = 0f;
			transform.rotation = Quaternion.Euler(angles);
		}
		else if (Input.GetMouseButtonDown(1))
		{
			transform.rotation = toRotation = targetCharacter.rotation;
		}
	}
}

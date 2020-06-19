﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
	[Range(0, 1)]
	public float process;
	public MeshDeformation[] targets;
	public Vector3 originalPosition;
	public Vector3 targetPosition;

	private void Start()
	{
		originalPosition = transform.position;
	}

	private void Update()
	{
		for (int i = 0; i < targets.Length; i++)
			targets[i].process = process;
		transform.position = Vector3.Lerp(originalPosition, targetPosition, process);
	}
}

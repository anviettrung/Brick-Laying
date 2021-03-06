﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInt : UnityEvent<int> { }

public class MeshDeformation : MonoBehaviour
{
	[Range(0, 1)]
	public float value;
	public bool updateRealtime;
	private float lastValue = -1;

	protected Mesh deformingMesh;
	[HideInInspector] public Vector3[] originalVertices, displacedVertices;

	protected Vector3 center { get { return deformingMesh.bounds.center; } }
	protected float bottom { get { return deformingMesh.bounds.min.y; } }
	protected Vector3 oCenter;
	protected Vector3 extent;

	public static float EPSILON = 0.001f;

	public EventInt onUpdateVertex = new EventInt();

	protected virtual void Start()
	{
		deformingMesh = GetComponent<MeshFilter>().mesh;
		CalculateVertices();
	}
	protected virtual void CalculateVertices()
	{
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];

		oCenter = center;
		extent = deformingMesh.bounds.extents;
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
	}
	protected virtual void Update()
	{
		if (deformingMesh.vertices.Length != originalVertices.Length) {
			CalculateVertices();
			onUpdateVertex.RemoveAllListeners();
		}

		if (System.Math.Abs(lastValue - value) > EPSILON || updateRealtime) {
			for (int i = 0; i < displacedVertices.Length; i++) {
				UpdateVertex(i);
				onUpdateVertex.Invoke(i);
			}
			deformingMesh.vertices = displacedVertices;
			deformingMesh.RecalculateNormals();

			UpdateYPosition(value);
			lastValue = value;
		}
	}

	protected virtual void UpdateYPosition(float value) {}
	protected virtual void UpdateVertex(int i) { }
}

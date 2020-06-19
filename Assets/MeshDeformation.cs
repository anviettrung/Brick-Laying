using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
	[Range(0, 1)]
	public float process;
	public float rangeXZ;
	public float rangeY;
	public Vector3 endScale;
	protected Vector3 startScale;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;

	protected Vector3 center { get { return deformingMesh.bounds.center; } }
	protected float bottom { get { return deformingMesh.bounds.min.y; } }
	protected float baseCenterToBot;

	public Vector3 extent;

	private void Start()
	{
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;

		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}

		baseCenterToBot = center.y - bottom;
		startScale = transform.localScale;
	}

	private void Update()
	{
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex(i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();

		UpdateYPosition(process);
	}

	public void UpdateYPosition(float value)
	{
		transform.localScale = Vector3.Lerp(startScale, endScale, value);
		Vector3 newPos = Vector3.zero;
		newPos.y = -(1 - transform.localScale.y) * baseCenterToBot;
		transform.localPosition = newPos;
	}

	private void UpdateVertex(int i)
	{
		// XZ plane
		float deltaXZ = (originalVertices[i].y - center.y) / extent.y;

		Vector3 outDirection = (originalVertices[i] - center);
		outDirection.y = 0;

		// Y Axis
		float deltaY = 1 - Mathf.Abs(deltaXZ);


		displacedVertices[i] = originalVertices[i] 
			+ outDirection.normalized * process * (-deltaXZ * deltaXZ + 1) * rangeXZ
			- transform.up * process * deltaY * rangeY;
	}
}

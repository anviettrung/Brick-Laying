using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
	[Range(0, 1)]
	public float value;
	public float rangeXZ;
	public float rangeY;
	public Vector3 endScale;
	protected Vector3 startScale;

	public BoxCollider boxCollider;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	float[] deltaXZ;

	protected Vector3 center { get { return deformingMesh.bounds.center; } }
	protected float bottom { get { return deformingMesh.bounds.min.y; } }
	protected float baseCenterToBot;

	public Vector3 extent;
	public Vector3 Extent { get { return deformingMesh.bounds.extents; } }

	private void Start()
	{
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;

		displacedVertices = new Vector3[originalVertices.Length];
		deltaXZ = new float[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
			deltaXZ[i] = (originalVertices[i].y - center.y) / extent.y;
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

		UpdateYPosition(value);
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
		float delta_xz = (originalVertices[i].y - center.y) / Extent.y;

		Vector3 outDirection = (originalVertices[i] - center);
		outDirection.y = 0;

		// Y Axis
		float deltaY = 1 - Mathf.Abs(delta_xz);


		displacedVertices[i] = originalVertices[i] 
			+ outDirection.normalized * value * (-delta_xz * delta_xz + 1) * rangeXZ
			- transform.up * value * deltaY * rangeY;
	}
}

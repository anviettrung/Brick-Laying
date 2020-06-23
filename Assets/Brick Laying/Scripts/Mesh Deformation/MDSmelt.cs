using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDSmelt : MeshDeformation
{
	public float rangeXZ;
	public float rangeY;
	public float xModifier = 0.2f;
	public Vector3 endScale;
	protected Vector3 startScale;

	public bool updateY = false;
	protected float[] deltaXZ;
	protected float baseCenterToBot;

	protected override void Start()
	{
		base.Start();

		baseCenterToBot = center.y - bottom;
		startScale = transform.localScale;
	}
	protected override void CalculateVertices()
	{
		base.CalculateVertices();

		deltaXZ = new float[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			deltaXZ[i] = (originalVertices[i].y - oCenter.y) / extent.y; // [-1, 1]
		}
	}
	protected override void UpdateYPosition(float value)
	{
		if (updateY) {
			transform.localScale = Vector3.Lerp(startScale, endScale, value);
			Vector3 newPos = Vector3.zero;
			newPos.y = -(1 - transform.localScale.y) * baseCenterToBot;
			transform.localPosition = newPos;
		}
	}
	protected override void UpdateVertex(int i)
	{
		// XZ plane
		float delta_xz = deltaXZ[i];
		if (System.Math.Abs(delta_xz) < EPSILON) delta_xz = 0;
		if (Mathf.Abs(delta_xz) >= 1 - EPSILON) delta_xz = 1 * Mathf.Sign(delta_xz);

		Vector3 outDirection = (originalVertices[i] - oCenter);
		outDirection.y = 0;
		outDirection.x *= xModifier;

		// Y Axis
		float deltaY = Mathf.Abs(delta_xz);
		if (delta_xz >= 0)
			deltaY = 1 - deltaY;
		else if (delta_xz > -0.5f)
			deltaY = 1 + deltaY * 1.5f;
		else
			deltaY = 4 * (1 - deltaY);

		outDirection = outDirection.normalized * value * (-delta_xz * delta_xz + 1) * rangeXZ;
		//outDirection = Quaternion.Euler(rangeY * (1-Mathf.Abs(delta_xz)), 0, 0) * outDirection;

		displacedVertices[i] = originalVertices[i] + outDirection - Vector3.up * value * deltaY * rangeY;
	}
}

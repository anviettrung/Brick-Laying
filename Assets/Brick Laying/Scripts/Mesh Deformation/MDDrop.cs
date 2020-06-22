using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDDrop : MeshDeformation
{
	public enum Direction
	{
		LeftToRight,
		RightToLeft
	}
	public float dropTime;
	public float accelerateY;
	public Direction direction;
	public bool xAxis = true;

	protected override void UpdateVertex(int i)
	{
		float xLocation = 0;
		//((originalVertices[i].x - oCenter.x) / extent.x) : (originalVertices[i].z - oCenter.z) / extent.z)
		xLocation = xAxis ? ((originalVertices[i].x - oCenter.x) / extent.x) : ((originalVertices[i].z - oCenter.z) / extent.z);
		xLocation = (xLocation + 1) * 0.5f;
		float delta = 0;

		switch (direction) {
			case Direction.LeftToRight:
				delta = dropTime * Mathf.Abs(xLocation - value);
				delta = xLocation < value ? delta : 0;
				break;
			case Direction.RightToLeft:
				delta = dropTime * Mathf.Abs(1 - xLocation - value);
				delta = 1 - xLocation < value ? delta : 0;
				break;
		}

		displacedVertices[i] = originalVertices[i] - transform.up * accelerateY * delta * delta * 0.5f;
	}
}

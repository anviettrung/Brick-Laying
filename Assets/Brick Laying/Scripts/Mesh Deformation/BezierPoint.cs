using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
	public Color color;
	public float size;
	public BezierPoint connector;

	void OnDrawGizmos()
	{
		// Draw a sphere at the transform's position
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, size);

		if (connector) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, connector.transform.position);
		}
	}

}

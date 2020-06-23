using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDBezier : MeshDeformationExtent
{
	public Transform limit;
	public Transform[] bezierPoint; // 4 points

	public override void UpdateVertex(int i)
	{
		MDSmelt mdsmelt = (MDSmelt)baseMD;
		if (limit.localPosition.z > baseMD.displacedVertices[i].z) {
			float t = (mdsmelt.deltaXZ[i] + 1) * 0.5f; // [-1;1] -> [0,1]

			Vector3 targetPos = MathU.CalculateCubicBezierPoint(t, 
				bezierPoint[0].localPosition, 
				bezierPoint[1].localPosition, 
				bezierPoint[2].localPosition, 
				bezierPoint[3].localPosition);
			targetPos.x = baseMD.originalVertices[i].x;

			baseMD.displacedVertices[i] = Vector3.Lerp(baseMD.displacedVertices[i], targetPos, baseMD.value);
		}
	}
}

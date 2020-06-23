using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformationExtent : MonoBehaviour
{
	public MeshDeformation baseMD;

	private void Awake()
	{
		baseMD.onUpdateVertex.AddListener(UpdateVertex);
	}

	public virtual void UpdateVertex(int i)
	{

	}
}

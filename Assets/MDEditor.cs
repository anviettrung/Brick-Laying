using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshDeformation))]
public class MDEditor : Editor
{
	MeshDeformation mTarget;

	private void Awake()
	{
		mTarget = (MeshDeformation)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();


	}
}

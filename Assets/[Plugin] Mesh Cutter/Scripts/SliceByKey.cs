using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceByKey : MonoBehaviour
{
	public MouseSlice mouseSlice;
	public GameObject objToSlice;

	public void Start()
	{
		mouseSlice.onSlicedMesh.AddListener((o, t) => {
			if (t) {
				o.GetComponent<MDSmelt>().value = 0;
				Destroy(o.GetComponent<MDSmelt>());
				o.GetComponent<MDDrop>().enabled = true;
			}
		});
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log("Slice");
			mouseSlice.Slice(objToSlice, transform.position, transform.right);
		}
	}
}

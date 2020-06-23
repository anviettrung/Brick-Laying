using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EzySlice;

public class EventSlice : UnityEvent<GameObject, bool> { }

public class SliceByKey : MonoBehaviour
{
	public  GameObject[] objToSlice;

	public EventSlice onSliceMesh = new EventSlice();

	public void Start()
	{
		onSliceMesh.AddListener((o, upper) => {
			o.GetComponent<MDSmelt>().value = 0;
			Destroy(o.GetComponent<MDSmelt>());
			if (upper) {
				MDDrop mdDrop = o.GetComponent<MDDrop>();
				mdDrop.enabled = true;
				StartCoroutine(CoroutineUtils.Chain(
					CoroutineUtils.LinearAction(mdDrop.dropTime, (w) => {
						mdDrop.value = w;
					}),
					CoroutineUtils.Do(() => {
						o.GetComponent<Rigidbody>().useGravity = true;
					})
				));
			} else {
				MeshDeformation[] md = o.GetComponents<MeshDeformation>();
				MeshDeformationExtent[] mde = o.GetComponents<MeshDeformationExtent>();

				for (int i = 0; i < md.Length; i++)
					Destroy(md[i]);

				for (int i = 0; i < mde.Length; i++)
					Destroy(mde[i]);
			}
		});
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			for (int i = 0; i < objToSlice.Length; i++)
				Slice(objToSlice[i], transform.position, transform.right);
		}
	}

	/*
	 * Example on how to slice a GameObject in world coordinates.
	 */
	public GameObject[] Slice(GameObject objectToSlice, Vector3 planeWorldPosition, Vector3 planeWorldDirection)
	{
		Material mat = objectToSlice.GetComponent<Renderer>().sharedMaterial;
		Material[] mats = new Material[2];
		mats[0] = mat;
		mats[1] = mat;

		GameObject[] res = new GameObject[2];
		SlicedHull hull = objectToSlice.Slice(planeWorldPosition, planeWorldDirection);

		for (int i = 0; i < res.Length; i++) {

			res[i] = Instantiate(objectToSlice, objectToSlice.transform.parent);
			res[i].name = objectToSlice.name + " (" + i.ToString() + ")";
			res[i].GetComponent<MeshFilter>().mesh = i == 0 ? hull.upperHull : hull.lowerHull;
			res[i].GetComponent<MeshRenderer>().materials = mats;
		
		}

		onSliceMesh.Invoke(res[0], true);
		onSliceMesh.Invoke(res[1], false);

		objectToSlice.SetActive(false);

		return res;
	}
}

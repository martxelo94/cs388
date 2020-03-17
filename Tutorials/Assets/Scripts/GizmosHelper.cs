using UnityEngine;
using System.Collections;

public class GizmosHelper : MonoBehaviour {

	public float arrowLength;
	public float arrowWidth;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnDrawGizmos(){
		if (gameObject == null)
			return;
		Gizmos.color = Color.black;
		//float cilinderRadius = 1;
		//float coneRadius = 1;
		//float coneHeith = 1.5f;
		Vector3[] arrowPoints = {
			transform.position,
			transform.position + Vector3.up * arrowLength,
			transform.position + Vector3.up * arrowLength / 3 + Vector3.left * arrowWidth,
			transform.position + Vector3.up * arrowLength / 3 + Vector3.right * arrowWidth
		};

		Gizmos.DrawLine (arrowPoints [0], arrowPoints [1]);
		Gizmos.DrawLine (arrowPoints [1], arrowPoints [2]);
		Gizmos.DrawLine (arrowPoints [1], arrowPoints [3]);
	}
}

using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	//Targets
	public Transform target;
	Quaternion rotation;
	
	//Camera settings
	public float speed = 10;
	public float xSpeed = 125;
	public float ySpeed = 75;
	private float distance;
	private float x = 0.0f; private float y = 0.0f;
	
	//Zoom settings
	public float zoomSpeed = 1;
	
	void Start () {
		rotation = transform.rotation;
	}
	
	void Update () {
		MoveCamera ();
		SelectTarget ();
	}
	
	void MoveCamera(){
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
			transform.position += rotation * Vector3.up * Mathf.Sign (Input.GetAxis("Vertical")) * speed * Time.deltaTime;
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
			transform.position +=  Vector3.right * Mathf.Sign (Input.GetAxis("Horizontal")) * speed * Time.deltaTime;
		if(Input.GetAxis ("Mouse ScrollWheel") != 0)
			transform.position += rotation * Vector3.forward * zoomSpeed * Time.deltaTime * Mathf.Sign (Input.GetAxis ("Mouse ScrollWheel"));
		
		if (Input.GetMouseButton(1)) { 
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			if(target != null){
				float orbitDistance = Vector3.Distance (target.position, transform.position);
				rotation = Quaternion.Euler(y, x, 0) ;
				transform.rotation = rotation;
				transform.position = rotation * new Vector3(0, 0, -orbitDistance) + target.position;
			}
			else{
				rotation = Quaternion.Euler(y, x, 0) ;
				transform.rotation = rotation;
			}
		}
	}
	
	
	void SelectTarget(){
		if(Input.GetMouseButtonUp(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				target = hit.transform;
			}
			if(!Physics.Raycast(ray, out hit)){
				target = null;
			}
			
		}
		if(target != null){
			if(Input.GetKey(KeyCode.F)){
				transform.position = Vector3.Lerp (transform.position, (target.position + transform.position).normalized * 4, 0.1f);
			}
			//Debug.DrawLine (target.position, transform.position, Color.red);
		}
	}
	
	
}

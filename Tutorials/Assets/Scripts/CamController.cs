using UnityEngine;
#if UNITY_EDITOR //Only execute if Editor
public class CamController : MonoBehaviour {

	public bool controlActive = true;   //If true camera controller will be updated
	public float mouseSensibility = 1f; //Sets camera rotation speed modifier
	public bool invertMouseY	  = true; //Allows inverting Y axis

	private Vector3 tempRotation;  //Temporary rotation over frame
	Vector3 oldMousePos = new Vector3(-1f,-1f,-1f); //Position of mouse on the last frame
	// Update is called once per frame
	void Update () {
		if(!controlActive) return;  //Returns if script deactivated
		tempRotation = Vector3.zero; //Inits frame rotation
	    UpdateMouse();  //Updates mouse input
		DoRotate(); //Sets the new obj rotation
	}

	// Fills temprotation with a value depending on mouse movement from the last frame
	void UpdateMouse(){
        Debug.Log("CamController update");
		// Security check for the first input (otherwise wrong init)
		if(oldMousePos.x < 0f )
		{
			oldMousePos = Input.mousePosition;
			return;
		}
		//Movement of the mouse (can be given by Input library)
		Vector3 deltarot 	= Input.mousePosition - oldMousePos;
		//Updates frame mouse position
		oldMousePos 		= Input.mousePosition;
		//Check to invert Y axis
		int mouseY = 1;
		if (invertMouseY) mouseY = -1;
		//Sets temp rotation with the Input 
	    tempRotation = transform.rotation.eulerAngles + mouseSensibility * new Vector3(deltarot.y*mouseY, deltarot.x, 0f );
	}
		
	// Applies the rotation to the object
	void DoRotate(){ 
		if (tempRotation == Vector3.zero) return;
		if (tempRotation.x > 90 && tempRotation.x < 180) tempRotation.x = 89;
		if (tempRotation.x >180 && tempRotation.x < 270) tempRotation.x = 270; 
		transform.rotation = Quaternion.Euler( tempRotation );
	}
}
#endif

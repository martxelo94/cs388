using UnityEngine;
//This class updates VR camera configurations depending on player prefs
public class VRCameraConfig : MonoBehaviour {
	public Transform leftEye;  //left eye vr camera
	public Transform rightEye; //Right eye vr camera
	//Fisheye scripts. Is an array because we might include more than l/r cameras
	public UnityStandardAssets.ImageEffects.Fisheye[] fisheyes;
	//If false is a config camera. If true is automatically updated
	public bool startUpdating = true;
	//Slider for eye separation
	public UnityEngine.UI.Slider eyeSlider;
	//slider for barrel distorsion
	public UnityEngine.UI.Slider barrelSlider;
	// Use this for initialization
	void Start () {
		//If true we read the config from player preferences and apply
		//If false we init with the values from sliders
		if (startUpdating) {
			UpdateDistance ();
			UpdateBarrelDistorsion ();
		} else
			SetupValues ();
	}
	//Sets up values from the sliders
	//Not efficient but works for demos
	public void SetupValues(){
		PlayerPrefs.SetFloat ("EyeSeparation", eyeSlider.value);
		PlayerPrefs.SetFloat ("BarrelDistorsion", barrelSlider.value);
		UpdateDistance ();
		UpdateBarrelDistorsion ();
	}
	//Updates eye separation from PlayerPrefs from 0 to 2
	public void UpdateDistance(){
		float separation = PlayerPrefs.GetFloat ("EyeSeparation") / 2f;
		leftEye.transform.localPosition  =  new Vector3 (-separation, 0f, 0f);
		rightEye.transform.localPosition  = new Vector3 (separation, 0f, 0f);
	}
	//Updates X value of barrel distorsion from playerPrefs from 0 to 1.5
	public void UpdateBarrelDistorsion(){
		float distorsion = 1.5f*PlayerPrefs.GetFloat ("BarrelDistorsion");
		for (int i = 0; i < fisheyes.Length; i++)
			fisheyes [i].strengthX = distorsion;
	}
}

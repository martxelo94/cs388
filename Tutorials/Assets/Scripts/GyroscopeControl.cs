using UnityEngine;
//Only executed when Platform is set to Android
#if UNITY_ANDROID
public class GyroscopeControl : MonoBehaviour
{
	//Objects transform to change
	Transform transformObject;
	#region [Private fields]
		private bool gyroEnabled = false;
		private const float lowPassFilterFactor = 0.2f;
		private readonly Quaternion baseIdentity = Quaternion.Euler (90, 0, 0);
		private Quaternion cameraBase = Quaternion.identity;
		private Quaternion calibration = Quaternion.identity;
		private Quaternion baseOrientation = Quaternion.Euler (90, 0, 0);
		private Quaternion baseOrientationRotationFix = Quaternion.identity;
		private Quaternion referanceRotation = Quaternion.identity;
	#endregion
	#region [Unity events]
		protected void Start ()
		{
			transformObject = gameObject.transform;
			Input.gyro.enabled = true;
			AttachGyro ();
		}
		protected void LateUpdate ()
		{
				if (gyroEnabled) {
						transformObject.localRotation = Quaternion.Slerp (transformObject.localRotation, cameraBase * (ConvertRotation (referanceRotation * Input.gyro.attitude) * GetRotFix ()), lowPassFilterFactor);
				}
		}
	#endregion
	#region [Public methods]
		private void AttachGyro ()
		{
				gyroEnabled = true;
				ResetBaseOrientation ();
				UpdateCalibration (true);
				UpdateCameraBaseRotation (true);
				RecalculateReferenceRotation ();
		}
		private void DetachGyro ()
		{
				gyroEnabled = false;
		}
	#endregion
	#region [Private methods]
		private void UpdateCalibration (bool onlyHorizontal)
		{
				if (onlyHorizontal) {
						var fw = (Input.gyro.attitude) * (-Vector3.forward);
						fw.z = 0;
						if (fw == Vector3.zero) {
								calibration = Quaternion.identity;
						} else {
								calibration = (Quaternion.FromToRotation (baseOrientationRotationFix * Vector3.up, fw));
						}
				} else {
						calibration = Input.gyro.attitude;
				}
		}
		private void UpdateCameraBaseRotation (bool onlyHorizontal)
		{
				if (onlyHorizontal) {
						var fw = transformObject.forward;
						fw.y = 0;
						if (fw == Vector3.zero) {
								cameraBase = Quaternion.identity;
						} else {
								cameraBase = Quaternion.FromToRotation (Vector3.forward, fw);
						}
				} else {
						cameraBase = transformObject.localRotation;
				}
		}
		private static Quaternion ConvertRotation (Quaternion q)
		{
				return new Quaternion (q.x, q.y, -q.z, -q.w);	
		}
		private Quaternion GetRotFix ()
		{
				return Quaternion.identity;
		}
		private void ResetBaseOrientation ()
		{
				baseOrientationRotationFix = GetRotFix ();
				baseOrientation = baseOrientationRotationFix * baseIdentity;
		}
		private void RecalculateReferenceRotation ()
		{
				referanceRotation = Quaternion.Inverse (baseOrientation) * Quaternion.Inverse (calibration);
		}
	#endregion
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Aim : MonoBehaviour {

	[SerializeField] Rigidbody rigSource;
	[SerializeField] Transform handleSource;
	[SerializeField] Vector3 addForce;
	[SerializeField] Transform testArrow;
	[SerializeField] float angle;
	float distanceMax = 15f;
	float distanceNow = 15f;
	Vector3 startPosition;
	[SerializeField] Transform lockTransform;

	[SerializeField] [Range(0,1)] float myRange = 0.5f;
	[SerializeField] bool debugShoot;
	[SerializeField] bool debugReset;
	[SerializeField] bool debugBetter;
	void Awake(){
		
	}

	void Start(){
		if (rigSource != null) {
			rigSource.useGravity = false;
//			startPosition = rigSource.transform.position;
			rigSource.isKinematic = true;
		}
	}

	void Update()
	{
		if (debugShoot) {
			Shoot ();
			debugShoot = false;
		}
		if (debugReset) {
			Reset ();
			debugReset = false;
		}
		angle = Mathf.Atan2 (addForce.y, addForce.z) * Mathf.Rad2Deg;
		Ray _ray = new Ray (this.transform.position, this.transform.forward);
		RaycastHit _hit;
		if (Physics.SphereCast (_ray,3f, out _hit, distanceMax, 1 << LayerMask.NameToLayer ("Actor"))) {
			lockTransform = _hit.collider.transform;
			distanceNow = _hit.distance;
		} else {
			lockTransform = null;
		}

	}

	void Shoot(){
		if (rigSource != null) {
			rigSource.useGravity = true;
			rigSource.drag = 0;
			rigSource.isKinematic = false;

			Vector3 _finalForce = this.transform.TransformDirection (addForce);

			float _gravity = 9.8f;

			if (lockTransform != null && debugBetter) {
				float _h = handleSource.position.y;
				float _t = Mathf.Sqrt ((2 * _h) / _gravity);
				float _rad = Mathf.Deg2Rad * 20f;
				Vector3 _direction = lockTransform.position - handleSource.position;
				float _distance = _direction.magnitude;

				testArrow.LookAt (lockTransform);
//				testArrow.localEulerAngles = new Vector3(testArrow.localEulerAngles.x - 20f,testArrow.localEulerAngles.y,testArrow.localEulerAngles.z);

				Vector3 _directionShoot = Quaternion.AngleAxis(-20f,testArrow.right) * testArrow.forward;

//				Debug.DrawRay (handleSource.position, _directionShoot.normalized * 10f,Color.blue,5f);
				float _v0 = Mathf.Sqrt((_distance * _gravity)  / (2 * Mathf.Sin(_rad) * Mathf.Cos(_rad)));


				_finalForce = _v0 * _directionShoot.normalized;


			} 
			//default shoot
			rigSource.AddForce (_finalForce, ForceMode.Impulse);
		}
	}

	void Reset(){
		if (rigSource != null) {
			rigSource.useGravity = false;
			rigSource.drag = 20;
			rigSource.isKinematic = true;
			if (handleSource != null) {
				rigSource.transform.localPosition = Vector3.zero;
			}
			testArrow.localEulerAngles = Vector3.zero;
//			rigSource.transform.position = startPosition;
		}
	}

	void OnDrawGizmos(){

		if (rigSource == null)
			return;

		if (handleSource == null)
			return;
		Gizmos.color = Color.green;
		Gizmos.DrawRay (handleSource.position, this.transform.TransformDirection (addForce));

		Gizmos.color = Color.yellow;
		if (lockTransform != null) {
			Gizmos.color = Color.red;
		}
		Gizmos.DrawRay (this.transform.position, this.transform.forward * distanceNow);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{

	const float minDistance = 10f;

	public enum MOVE_TYPE
	{
		LOCK_TARGET,
		FREE
	}

	public MOVE_TYPE moveType = MOVE_TYPE.LOCK_TARGET;

	public float distance = 20f;

	public float speed = 240;

	public bool enableControl = true;

	void Start()
	{
		if (moveType == MOVE_TYPE.LOCK_TARGET) {
			
		}
	}

	// Update is called once per frame
//	void Update ()
//	{
//
//		enableControl = CheckCameraEnable ();
//
//		if (enableControl) {
//			MoveCamera ();
//		}
//	}

	void LateUpdate(){
		enableControl = CheckCameraEnable ();

		if (enableControl) {
			MoveCamera ();
		}
	}

	
	bool CheckCameraEnable ()
	{
//		if (GameManager.Instance == null) {
//			return false;
//		}
//
//		if (GameManager.Instance.NowControler == null) {
//			return false;
//		}

		return true;
	}

	void MoveCamera ()
	{

		switch (moveType) {
		case MOVE_TYPE.LOCK_TARGET:
			MoveCamera_LockTarget ();
			break;
		case MOVE_TYPE.FREE:
			MoveCamera_Free ();
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
			
	}

	void MoveCamera_LockTarget ()
	{
		Transform targetTransform = GameManager.Instance.NowControler.PlayerEntity.ActorTransform;

//		this.transform.LookAt (targetTransform);


		//-------ROTATION----------

		if (Input.GetMouseButton (1)) {
			float _mX = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
			//			float _mY = Input.GetAxis ("Mouse Y") * speed * Time.deltaTime;

			Vector3 _rotateAxisX = Vector3.up;
			//			Vector3 _rotateAxisY = Vector3.right;

			transform.RotateAround (targetTransform.position, _rotateAxisX, _mX);

		}

		//-------ROTATION----------


		//-------ZOOM IN/OUT---------

		Vector3 _direction_player_2_camera = this.transform.position - targetTransform.position;
		float _mouseAxis = Input.GetAxis ("Mouse ScrollWheel");

		distance += _mouseAxis * (-10f);

		if (distance < minDistance) {
			distance = minDistance;
		}

		this.transform.position = targetTransform.position + _direction_player_2_camera.normalized * distance;

		//-------ZOOM IN/OUT---------


		this.transform.LookAt (targetTransform);


		//-------CLAMP-------------

		Vector3 _clampPosition = new Vector3 (this.transform.position.x, Mathf.Max (8f, this.transform.position.y), this.transform.position.z);

		this.transform.position = Vector3.MoveTowards (this.transform.position, _clampPosition, Time.deltaTime * 20f);

		//-------CLAMP-------------
	}

	void MoveCamera_Free()
	{

		float _y = this.transform.position.y;

		//-------ZOOM IN/OUT---------


		float _axis = Input.GetAxis ("Mouse ScrollWheel");
	
		this.transform.position = this.transform.position + this.transform.forward * _axis * 10f;


		//--------------------------

		//-------ROTATION----------

		if (Input.GetMouseButton (1)) {
			float _mX = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
			float _mY = Input.GetAxis ("Mouse Y") * speed * Time.deltaTime;

			Vector3 _euler = transform.eulerAngles + new Vector3 (_mY * (-1f), _mX, 0);

			Quaternion _mRotation = Quaternion.Euler (_euler);

			transform.rotation = _mRotation;

		}

		//--------------------------


		//-------TRANSLATION-------

		if (Input.GetMouseButton (2)) {
			float _mX = Input.GetAxis("Mouse X") * speed * Time.deltaTime * 0.3f;
			float _mY = Input.GetAxis ("Mouse Y") * speed * Time.deltaTime * 0.3f;

			Vector3 _endPosition = this.transform.position + this.transform.up * _mY *(-1f)  + this.transform.right * _mX * (-1f);

			this.transform.position = _endPosition;
		}

		//--------------------------


		//-------CLAMP-------------

		Vector3 _clampPosition = new Vector3 (this.transform.position.x, Mathf.Max (8f, this.transform.position.y), this.transform.position.z);

		this.transform.position = _clampPosition;

		//--------------------------

	}
}

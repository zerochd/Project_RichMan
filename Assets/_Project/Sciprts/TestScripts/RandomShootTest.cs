using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomShootTest : MonoBehaviour {

	[SerializeField] float radius = 1.5f;
	[SerializeField] int bullet = 6;

	[SerializeField] [Range(0,1f)] float accuary = 0.8f;
	[SerializeField] bool debugShoot;
	[SerializeField] bool debugReset;
	[SerializeField] Vector2[] vectorArray;
	// Update is called once per frame
	void Update () {
		if (debugShoot == true) {
			RandomShoot (radius);
			debugShoot = false;
		}

		if (debugReset == true) {
			Reset ();
			debugReset = false;
		}
	}

	void RandomShoot(float radius){
		vectorArray = new Vector2[bullet];
		float _singlePage = 360 / bullet;
		if (vectorArray != null) {
			Vector2 _randomVector2 = new Vector2();
			Vector3 _randomVector3 = new Vector3();
			float _distance;
			float _firstAcc = Random.Range(0,radius) * (1 - accuary);
			for (int i = 0; i < vectorArray.Length; i++) {

				_firstAcc += ((float)i / bullet)  * radius;
				_firstAcc = Mathf.Repeat (_firstAcc, radius);
				_firstAcc = Mathf.Lerp (radius * (1 - accuary), radius,_firstAcc / radius);
//				_firstAcc = Mathf.Lerp (_firstAcc, radius, (float)i / vectorArray.Length);
				if (i == 0) {
					_randomVector2 = Random.insideUnitCircle * _firstAcc;
					_distance = _randomVector2.magnitude;
					_randomVector3.Set(_randomVector2.x,_randomVector2.y,0);

				} else {
					_randomVector3 = Quaternion.AngleAxis (_singlePage, this.transform.up) * _randomVector3;
					_randomVector3 = _randomVector3.normalized *_firstAcc;

					_randomVector2.Set (_randomVector3.x, _randomVector3.y);
				}
//			
//				_randomVector2 = Random.insideUnitCircle * radius;
//				_randomVector3.Set(_randomVector2.x,_randomVector2.y,0);
				vectorArray[i].Set (_randomVector2.x, _randomVector2.y);
			}
		}
	}

	void Reset(){
		if (vectorArray != null) {
	
			for (int i = 0; i < vectorArray.Length; i++) {
				vectorArray[i].Set (-100, -100);
			}
		}
	}

	#if UNITY_EDITOR
	void OnDrawGizmos(){
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.DrawWireDisc (this.transform.position, this.transform.up, radius);

		if (vectorArray != null) {
			foreach (Vector2 vector in vectorArray) {
				Vector3 targetPoint = new Vector3 (vector.x, vector.y,0);
//				UnityEditor.Handles.DrawLine (targetPoint, this.transform.up);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (targetPoint, this.transform.up);
			}
		}
	}
	#endif
}

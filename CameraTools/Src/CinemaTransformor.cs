using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CameraTools
{
	public class CinemaTransformor : MonoBehaviour {

		public Transform PlaneTrans;

		public float PitchAngle = 30;

		private static float MinZoomDistance = 2;
		private static float MaxZoomDistance = 20;

		private static float MinPitchAngle = 25;
		private static float MaxPitchAngle = 500;

		[SerializeField]
		public Vector3 AnchorPosition;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		/// <summary>
		/// Determines whether this instance is valid. Use by Editor
		/// </summary>
		/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
		public bool IsValid(){
			return Vector3.Angle (PlaneTrans.transform.position - transform.position, AnchorPosition - transform.position) > 10;
		}

		/// <summary>
		/// Rotate the specified angle.
		/// </summary>
		/// <param name="angle">Angle.</param>
		public void Rotate(float angle){
			Vector3 point = Quaternion.AngleAxis(angle, PlaneTrans.up) * (transform.position - AnchorPosition);  
			Vector3 resultVec3 = AnchorPosition + point;  

			transform.position = resultVec3;

			UpdateRotate (AnchorPosition);
		}

		/// <summary>
		/// Transport the specified offset.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public Vector3 Transport(Vector3 offset){

			var projectOffset = Vector3.ProjectOnPlane (offset, PlaneTrans.up);

			if (PlaneTrans.GetComponent<Collider> ().bounds.Contains (AnchorPosition + projectOffset)) {
				AnchorPosition += projectOffset;
				transform.position += projectOffset;

				return AnchorPosition;
			} else {
				return transform.position;
			}
		}

		/// <summary>
		/// Zoom the specified offset.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public void Zoom(float offset){
			float curDistance = Vector3.Distance (AnchorPosition, transform.position);

			float newoffset = offset;
			if (offset > 0)
				newoffset = curDistance - offset > MinZoomDistance ? offset : MinZoomDistance - curDistance;
			else
				newoffset = curDistance - offset < MaxZoomDistance ? offset : MaxZoomDistance - curDistance;

			transform.position += transform.forward * newoffset;
		}

		/// <summary>
		/// Pitch the specified angle.
		/// </summary>
		/// <param name="angle">Angle.</param>
		public void Pitch(float angle){

			float newAngle = angle;
			if (angle < 0)
				newAngle = PitchAngle + angle > MinPitchAngle ? angle : MinPitchAngle - PitchAngle;
			else
				newAngle = PitchAngle + angle < MaxPitchAngle ? angle : MaxPitchAngle - PitchAngle;


			PitchAngle += newAngle;

			//set the pitch angle
			var projectDir = Vector3.ProjectOnPlane(transform.position, PlaneTrans.up) - AnchorPosition;

			float h = Vector3.Distance (transform.position, AnchorPosition) * Mathf.Sin (PitchAngle / 180 * Mathf.PI);

			float l = Vector3.Distance (transform.position, AnchorPosition) * Mathf.Cos (PitchAngle / 180 * Mathf.PI);



			transform.position = (h * PlaneTrans.up) +  (projectDir.normalized * l) + AnchorPosition;

			UpdateRotate (AnchorPosition);
		}


		/// <summary>
		/// Updates the rotate.
		/// </summary>
		/// <param name="position">Position.</param>
		public void UpdateRotate(Vector3 position)
		{
			transform.rotation = Quaternion.LookRotation (position - transform.position, PlaneTrans.up);
		}

		//	void OnDrawGizmos()
		//	{
		//		// Draw a yellow sphere at the transform's position
		//		Gizmos.color = Color.yellow;
		//		Gizmos.DrawSphere(transform.position, 1);
		//	}
	}


}

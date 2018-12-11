using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CameraTools
{
	public class MouseControlAnchor: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        public bool bPicthOn = true;
        public bool bZoomOn = true;

		public float PitchSpeed = 25;
		public float RotateSpeed = 5;
		public float ZoomSpeed = 1f;

		CinemaTransformor ciTrans;
		Vector3 oldPos;
		Vector3 oldAnchorPos;

		bool bShift = false;

		BoxCollider DragCollider;

		// Use this for initialization
		void Start () {
			ciTrans = FindObjectOfType<CinemaTransformor> ();

			DragCollider = gameObject.AddComponent<BoxCollider> ();
			DragCollider.isTrigger = true;
			DragCollider.size = new Vector3 (2, 1, 2);
			DragCollider.enabled = false;
		}


		void Update(){
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && bZoomOn)
			{
				ciTrans.Zoom (Input.GetAxis ("Mouse ScrollWheel") * ZoomSpeed);
			}
		}

		/// <summary>
		/// Raises the begin drag event.
		/// </summary>
		/// <param name="data">Data.</param>
		public void OnBeginDrag(PointerEventData data)
		{
			bShift = Input.GetKey (KeyCode.LeftShift);
			oldPos = data.pointerPressRaycast.worldPosition;

			oldAnchorPos = ciTrans.AnchorPosition;
			DragCollider.enabled = true;
		}

		/// <summary>
		/// Raises the drag event.
		/// </summary>
		/// <param name="data">Data.</param>
		public void OnDrag(PointerEventData data)
		{
			DoDrag (data);
		}

		/// <summary>
		/// Raises the end drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData)
		{
			
			DoDrag (eventData);
			DragCollider.enabled = false;
			bShift = false;
		}

		/// <summary>
		/// Dos the drag.
		/// </summary>
		/// <param name="data">Data.</param>
		void DoDrag(PointerEventData data){

			//Debug.Log ("Do drag press postion " + oldPos);
			var offset = GetProjectOffset (data);
			//transform.position += offset;

            if (data.button == PointerEventData.InputButton.Left && bPicthOn) {

				if (!bShift)
					ciTrans.Transport (-offset);
				else {
					ciTrans.Pitch (offset.y * PitchSpeed);
					UpdateOldPos (data);
				}
			}
			else if (data.button == PointerEventData.InputButton.Right
                     || (PointerEventData.InputButton.Left == data.button && !bPicthOn)) {
				ciTrans.Rotate (GetRotateOffset(offset) * RotateSpeed);
				UpdateOldPos (data);
			}
				
		}

		void UpdateOldPos(PointerEventData data)
		{
			if (data.pointerCurrentRaycast.isValid == false)
				return;
			oldPos = data.pointerCurrentRaycast.worldPosition;
		}

		/// <summary>
		/// Gets the rotate offset.
		/// </summary>
		/// <returns>The rotate offset.</returns>
		/// <param name="posOffset">Position offset.</param>
		float GetRotateOffset(Vector3 posOffset){
			bool bClockWise = Vector3.Dot (Vector3.Cross (posOffset + oldPos - oldAnchorPos, oldPos - oldAnchorPos), transform.up) > 0;
			return bClockWise ? posOffset.magnitude : -posOffset.magnitude;
		}

		/// <summary>
		/// Gets the project offset.
		/// </summary>
		/// <returns>The project offset.</returns>
		/// <param name="data">Data.</param>
		Vector3 GetProjectOffset(PointerEventData data){
			if (data.pointerCurrentRaycast.isValid == false)
				return Vector3.zero;

			var newPos = data.pointerCurrentRaycast.worldPosition;

			var offset = newPos - oldPos; 

			var projectOffset = Vector3.ProjectOnPlane(offset, transform.up);

			//oldPos = newPos + offset;  //
			//Debug.Log ("GetProjectOffset " + projectOffset);
			return projectOffset;
		}
	}
}


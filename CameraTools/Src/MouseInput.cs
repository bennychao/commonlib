using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CameraTools
{
	public class MouseInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		CinemaTransformor ciTrans;
		// Use this for initialization
		void Start () {
			ciTrans = GetComponent<CinemaTransformor> ();
		}

		public void OnBeginDrag(PointerEventData data)
		{
			var button = Input.GetKey (KeyCode.LeftShift);
			Debug.Log ("OnBeginDrag " + data.button.ToString() + " left " + button.ToString());
			//oldPos = data.enterEventCamera.ScreenToWorldPoint (data.position);
			//TestAnchor.transform.position = data.pointerPressRaycast.worldPosition;
			oldPos = data.pointerPressRaycast.worldPosition;
		}

		Vector3 oldPos;
		public void OnDrag(PointerEventData data)
		{
			//Debug.Log ("OnDrag " + data.pointerCurrentRaycast.worldPosition + " valid " + data.pointerCurrentRaycast.isValid);
			Debug.Log ("OnDrag " + data.delta);

			//var newPos = Camera.main.ScreenToWorldPoint (new Vector3(data.position.x, data.position.y, Camera.main.nearClipPlane));
			var newPos = data.pointerCurrentRaycast.worldPosition;

			var offset = newPos - oldPos; 
			Debug.Log ("offset is " + offset);
			var projectOffset = Vector3.ProjectOnPlane(offset, data.pointerCurrentRaycast.worldNormal);
			transform.position += projectOffset;

			//var dir2 = Camera.main.worldToCameraMatrix.inverse.MultiplyPoint (data.delta) - Camera.main.transform.position;
			//

			//		var projectDir = Vector3.ProjectOnPlane(dir2, data.pointerCurrentRaycast.worldNormal);
			//		Debug.Log ("dir = " + dir2.ToString() + "  projectDir = " + projectDir.ToString());
			//TestAnchor.transform.position += projectDir;
			oldPos = newPos;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log ("OnEndDrag");
		}

	}
}


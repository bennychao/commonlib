using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace CameraTools
{
	[CustomEditor(typeof(CinemaTransformor))]
	public class CinemaTransformorEditor : UnityEditor.Editor  {

		private CinemaTransformor cTrans = null;

		void OnEnable()
		{
			cTrans = target as CinemaTransformor;
			Debug.Log ("cTrans = target as CinemaTransformor; " + cTrans);

			CheckInitState ();
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			cTrans = target as CinemaTransformor;
		}

		void CheckInitState()
		{
			if (cTrans != null && !cTrans.IsValid ()) {

				AdjustInitState ();
			}
		}

		void AdjustInitState()
		{
			serializedObject.Update ();

			var posPort =  serializedObject.FindProperty("AnchorPosition");
			posPort.vector3Value = cTrans.PlaneTrans.transform.position;

			cTrans.UpdateRotate (posPort.vector3Value);
			//set the pitch angle
			//cTrans.Pitch(0);

			serializedObject.ApplyModifiedProperties();
		}

		void OnSceneGUI()
		{
			var posPort =  serializedObject.FindProperty("AnchorPosition");
			Vector3 position = posPort.vector3Value;
			if (Tools.current == Tool.Move)
			{

				EditorGUI.BeginChangeCheck();

				Quaternion rotation =Quaternion.identity;
				float size = HandleUtility.GetHandleSize(position) * 0.1f;

				Handles.SphereHandleCap(0, position, rotation, size, EventType.Repaint);

				Vector3 pos = Handles.PositionHandle(position, rotation);

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(target, "Move Waypoint");

					Vector3 offset = pos - posPort.vector3Value;
					//				posPort.vector3Value = pos;
					//				cTrans.UpdateRotate (pos);

					var endPos = cTrans.Transport (offset);
					posPort.vector3Value = endPos;

					serializedObject.ApplyModifiedProperties();
					//				wp.position = pos;
					//				Target.m_Waypoints[i] = wp;
					//				Target.InvalidateDistanceCache();
				}
			}
		}

	}

}

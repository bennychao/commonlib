using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CameraTools
{
	public class KeyBoardInput : MonoBehaviour {

		CinemaTransformor ciTrans;
		// Use this for initialization
		void Start () {
			ciTrans = GetComponent<CinemaTransformor> ();
		}

		// Update is called once per frame
		void Update () {
			//for test
			if (Input.GetKey (KeyCode.A))
				ciTrans.Rotate (1.1f);
			if (Input.GetKey (KeyCode.D))
				ciTrans.Rotate (-1.1f);

			if (Input.GetKey (KeyCode.S))
				ciTrans.Zoom (0.1f);
			if (Input.GetKey (KeyCode.W))
				ciTrans.Zoom (-0.1f);

			if (Input.GetKey (KeyCode.G))
				ciTrans.Pitch (0.1f);
			if (Input.GetKey (KeyCode.T))
				ciTrans.Pitch (-0.1f);

			if (Input.GetKey (KeyCode.F))
				ciTrans.Transport (0.1f * Vector3.forward);

			if (Input.GetKey (KeyCode.R))
				ciTrans.Transport (-0.1f * Vector3.forward);
		}
	}
}


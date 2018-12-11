using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CameraTools
{
    public class ScreenRotater : MonoBehaviour
    {

        public float RotateSpeed = 0.1f;

        CinemaTransformor ciTrans;
        bool bRotate = false;
        Vector3 oldPos = Vector3.zero;

        GraphicRaycaster[] canvasRaycaster;

        // Use this for initialization
        void Start()
        {
            ciTrans = FindObjectOfType<CinemaTransformor>();

            canvasRaycaster = FindObjectsOfType<GraphicRaycaster>();
        }

        public bool IsOnUI(Vector3 pos)
        {
            // = gameObject.GetComponent<GraphicRaycaster>();

            PointerEventData pData = new PointerEventData(EventSystem.current);

            pData.position = pos;// new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            pData.delta = Vector2.zero;
            pData.scrollDelta = Vector2.zero;


            foreach (var caster in canvasRaycaster)
            {
                List<UnityEngine.EventSystems.RaycastResult> canvasHits = new List<UnityEngine.EventSystems.RaycastResult>();
                caster.Raycast(pData, canvasHits);

                if (canvasHits.Count > 0)
                    return true;
            }


            //bool bFind = false;


            return false;
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.touchCount > 0){
            //    if (Input.GetTouch(0).phase == TouchPhase.Began){
            //        Debug.Log("Began Touch");
            //    }
            //}
            if (Input.GetMouseButtonUp(0))
            {
                // Debug.Log("Began Touch");
                bRotate = false;
            }
            else if (Input.GetMouseButtonDown(0) && !IsOnUI(Input.mousePosition))
            {
                bRotate = true;
                oldPos = Input.mousePosition;
            }

            if (bRotate)
            {
                var detal = Input.mousePosition - oldPos;

                var angle = RotateSpeed * detal.x;

                ciTrans.Rotate(angle);
                oldPos = Input.mousePosition;
            }
        }
    }

}

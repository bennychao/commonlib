using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CommonLib
{
    public static class CameraUtil
    {
        /// <summary>
        /// 返回一点到两眼中心平面（垂直方向上）的投影点
        /// </summary>
        /// <returns>The project on eyes normal plane.</returns>
        /// <param name="camera">Camera.</param>
        /// <param name="point">Point.</param>
        static public Vector3 PointProjectOnEyesVerticalPlane(Camera camera, Vector3 point)
        {
            Vector3 vec = point - camera.transform.position;

            //注意这里一定不要把Point直接代入，先得到平面上一点P，再把point到P的向量代入
            Vector3 pop = Vector3.ProjectOnPlane(vec, camera.transform.right);

            Vector3 pointop = pop + camera.transform.position;

            //Vector3.Project 这里需要注意两个函数的区别，一个是面平面投影，一个是向向量投影

            Debug.DrawLine(pointop, point, Color.red);

            return pointop;
        }


        /// <summary>
        /// 获取一点与可视矩阵的偏离度,垂直方向上的偏离度，其实是X轴方向上偏离
        /// </summary>
        /// <returns>The deviation to eyes.
        /// 如果返回值 >1 为偏出屏幕，不在可视区域内，否则在可视区域内
        /// </returns>
        /// <param name="camera">Camera.</param>
        /// <param name="point">Point.</param>
        static public float GetVerticalDeviationToEyes(Camera camera, Vector3 point)
        {
            Vector3 p = PointProjectOnEyesVerticalPlane(camera, point);

            float lPtoEye = Vector3.Project(p - camera.transform.position, camera.transform.forward).magnitude;

            //float lPtoEye = Vector3.Distance(p, camera.transform.position);

            float fov = GetHorizontalFOV(camera);

            float wCulled = lPtoEye * Mathf.Tan(fov / 2 / 180 * Mathf.PI);

            float w = Vector3.Distance(point, p);

            return w / wCulled;
        }



        /// <summary>
        /// Gets the horizontal fov.
        /// </summary>
        /// <returns>The horizontal fov.</returns>
        /// <param name="camera">Camera.</param>
        static public float GetHorizontalFOV(Camera camera){
            float fFov = 0f;

            float l = 1 / Mathf.Tan(camera.fieldOfView / 2 / 180 * Mathf.PI);
            float a = Mathf.Atan(Camera.main.aspect / l);

            return a / Mathf.PI * 180 * 2;

        }

        /// <summary>
        /// Gets the depth.就是对应Shader里的Depth，当然还是减去近平面的距离，一定要注意其不是到Camera的距离。
        /// </summary>
        /// <returns>The depth.</returns>
        /// <param name="camera">Camera.</param>
        /// <param name="point">Point.</param>
        static public float GetDepth(Camera camera, Vector3 point)
        {
            float lPtoEye = Vector3.Project(point - camera.transform.position, camera.transform.forward).magnitude;

            return lPtoEye;
        }

        /// <summary>
        /// Pulls the camera by vertical.在水平方向包含Point点
        /// </summary>
        /// <param name="camera">Camera.</param>
        /// <param name="point">需要Camera包含的点.</param>
        static public void PullCamera(Camera camera, Vector3 point){
            float fscale = GetVerticalDeviationToEyes(camera, point);
            float d = GetDepth(camera, point);

            d = d * (fscale - 1);

            camera.transform.position += -camera.transform.forward * d;

        }


        /// <summary>
        /// expend the camera by vertical.
        /// </summary>
        /// <param name="camera">Camera.</param>
        /// <param name="fScale">F scale.</param>
        static public void ExpendCameraByVertical(Camera camera, float fScale)
        {
            float a2 = Mathf.Atan(Mathf.Tan(camera.fieldOfView / 2 / 180 * Mathf.PI) * fScale);

            camera.fieldOfView = a2 / Mathf.PI * 180 * 2;
        }
    }

}

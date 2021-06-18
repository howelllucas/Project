// /************************************************************
// *                                                           *
// *   Mobile Touch Camera                                     *
// *                                                           *
// *   Created 2015 by BitBender Games                         *
// *                                                           *
// *   bitbendergames@gmail.com                                *
// *                                                           *
// ************************************************************/

using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace BitBenderGames
{

    /// <summary>
    /// A little helper script that allows to focus the camera on a transform either
    /// via code, or by wiring it up with one of the many events of the mobile touch camera
    /// or mobile picking controller.
    /// </summary>
    [RequireComponent(typeof(MobileTouchCamera))]
    public class FocusCameraOnItem : MonoBehaviourWrapped
    {

        [SerializeField]
        private float transitionDuration = 0.5f;

        [SerializeField]
        private AnimationCurve transitionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        [SerializeField]
        private float focusZoom;

        [SerializeField]
        private float resetZoom;

        [SerializeField]
        private Vector2 focusScreenPos = new Vector2(0.5f, 0.5f);

        private MobileTouchCamera MobileTouchCamera { get; set; }

        private Vector3 posTransitionStart;
        private Vector3 posTransitionEnd;
        private Quaternion rotTransitionStart;
        private Quaternion rotTransitionEnd;
        private float zoomTransitionStart;
        private float zoomTransitionEnd;
        private float timeTransitionStart;
        private bool isTransitionStarted;

        private Vector3 resetFocusPosition;

        public float TransitionDuration
        {
            get { return transitionDuration; }
            set { transitionDuration = value; }
        }

        public Vector2 FocusScreenPos
        {
            get { return focusScreenPos; }
            set { focusScreenPos = value; }
        }

        public void Awake()
        {
            MobileTouchCamera = GetComponent<MobileTouchCamera>();
            focusZoom = Mathf.Clamp(focusZoom, MobileTouchCamera.CamZoomMin, MobileTouchCamera.CamZoomMax);
            resetZoom = Mathf.Clamp(resetZoom, MobileTouchCamera.CamZoomMin, MobileTouchCamera.CamZoomMax);
            isTransitionStarted = false;
        }

        public void LateUpdate()
        {

            if (MobileTouchCamera.IsDragging || MobileTouchCamera.IsPinching)
            {
                timeTransitionStart = Time.time - transitionDuration;
            }

            if (isTransitionStarted == true)
            {
                if (Time.time < timeTransitionStart + transitionDuration)
                {
                    UpdateTransform();
                }
                else
                {
                    SetTransform(posTransitionEnd, rotTransitionEnd, zoomTransitionEnd);
                    isTransitionStarted = false;
                }
            }
        }

        private void UpdateTransform()
        {
            float progress = (Time.time - timeTransitionStart) / transitionDuration;
            float t = transitionCurve.Evaluate(progress);
            Vector3 positionNew = Vector3.Lerp(posTransitionStart, posTransitionEnd, t);
            Quaternion rotationNew;
            rotationNew = Quaternion.Lerp(rotTransitionStart, rotTransitionEnd, t);
            float zoomNew = Mathf.Lerp(zoomTransitionStart, zoomTransitionEnd, t);
            SetTransform(positionNew, rotationNew, zoomNew);
        }

        public void OnPickItem(RaycastHit hitInfo)
        {
            FocusCameraOnTransform(hitInfo.transform);
        }

        public void OnPickItem2D(RaycastHit2D hitInfo2D)
        {
            FocusCameraOnTransform(hitInfo2D.transform);
        }

        public void OnPickableTransformSelected(Transform pickableTransform)
        {
            FocusCameraOnTransform(pickableTransform);
        }

        public void FocusCameraOnTransform(Transform targetTransform)
        {
            if (targetTransform == null)
            {
                return;
            }
            FocusCameraOnTarget(targetTransform.position);
        }

        public void FocusCameraOnTransform(Vector3 targetPosition)
        {
            FocusCameraOnTarget(targetPosition);
        }

        public void FocusCameraOnTarget(Vector3 targetPosition)
        {
            resetFocusPosition = targetPosition;
            FocusCameraOnTarget(targetPosition, Transform.rotation, focusZoom);
        }

        public void ResetFocus()
        {
            FocusCameraOnTarget(resetFocusPosition, Transform.rotation, resetZoom);
        }

        private float GetTiltFromRotation(Quaternion camRotation)
        {
            Vector3 camForwardDir = camRotation * Vector3.forward;
            Vector3 camUp = MobileTouchCamera.CameraAxes == CameraPlaneAxes.XZ_TOP_DOWN ? Vector3.up : Vector3.back;
            Vector3 camRightEnd = Vector3.Cross(camUp, camForwardDir);
            Vector3 camForwardOnPlaneEnd = Vector3.Cross(camRightEnd, camUp);
            float camTilt = Vector3.Angle(camForwardOnPlaneEnd, camForwardDir);
            return camTilt;
        }

        public void FocusCameraOnTarget(Vector3 targetPosition, Quaternion targetRotation, float targetZoom)
        {

            timeTransitionStart = Time.time;
            posTransitionStart = Transform.position;
            rotTransitionStart = Transform.rotation;
            zoomTransitionStart = MobileTouchCamera.CamZoom;
            rotTransitionEnd = targetRotation;
            zoomTransitionEnd = targetZoom;

            MobileTouchCamera.CamZoom = targetZoom;
            MobileTouchCamera.Transform.rotation = targetRotation;
            Vector3 intersectionScreenCenterEnd = MobileTouchCamera.GetIntersectionPoint(MobileTouchCamera.Cam.ScreenPointToRay(new Vector3(Screen.width * focusScreenPos.x, Screen.height * focusScreenPos.y, 0)));
            Vector3 focusVector = targetPosition - intersectionScreenCenterEnd;
            posTransitionEnd = posTransitionStart + focusVector;

            NormalizePosAndRot(targetPosition, targetZoom, ref posTransitionEnd, ref rotTransitionEnd);

           
            posTransitionEnd = MobileTouchCamera.GetClampToBoundaries(posTransitionEnd, true);

            MobileTouchCamera.Transform.rotation = rotTransitionStart;
            MobileTouchCamera.CamZoom = zoomTransitionStart;

            if (Mathf.Approximately(transitionDuration, 0))
            {
                SetTransform(posTransitionEnd, rotTransitionEnd, zoomTransitionEnd);
                return;
            }

            isTransitionStarted = true;
        }

        private void NormalizePosAndRot(Vector3 focusPosition, float zoom, ref Vector3 pos, ref Quaternion rot)
        {
            float tiltTarget;
            if (MobileTouchCamera.EnableZoomTilt)
            {
                float zoomProgress = Mathf.Clamp01((zoom - MobileTouchCamera.CamZoomMin) / (MobileTouchCamera.CamZoomMax - MobileTouchCamera.CamZoomMin));
                tiltTarget = Mathf.Lerp(MobileTouchCamera.ZoomTiltAngleMin, MobileTouchCamera.ZoomTiltAngleMax, zoomProgress);
            }
            else
            {
                tiltTarget = MobileTouchCamera.GetCurrentTileAngleDeg();
            }
            float tiltTargetRadian = Mathf.Deg2Rad * tiltTarget;
            Vector3 right = MobileTouchCamera.Transform.right;
            Vector3 camForwardOnPlane = Vector3.Cross(MobileTouchCamera.RefPlane.normal, right);
            pos = focusPosition + camForwardOnPlane * zoom * Mathf.Cos(tiltTargetRadian) + MobileTouchCamera.RefPlane.normal * zoom * Mathf.Sin(tiltTargetRadian);
            Vector3 forward = (focusPosition - pos).normalized;
            Vector3 up = Vector3.Cross(forward, right);
            Matrix4x4 matrix = new Matrix4x4(right, up, forward, new Vector4(0, 0, 0, 1));
            rot = matrix.rotation;
            MobileTouchCamera.Transform.rotation = rot;
            Vector3 intersectionScreenFocusEnd = MobileTouchCamera.GetIntersectionPoint(MobileTouchCamera.Cam.ScreenPointToRay(new Vector3(Screen.width * focusScreenPos.x, Screen.height * focusScreenPos.y, 0)));
            Vector3 intersectionScreenCenterEnd = MobileTouchCamera.GetIntersectionPoint(MobileTouchCamera.Cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0)));
            pos += (intersectionScreenCenterEnd - intersectionScreenFocusEnd);

            MobileTouchCamera.CamZoom = zoom;
        }

        private void SetTransform(Vector3 newPosition, Quaternion newRotation, float newZoom)
        {
            Transform.position = newPosition;
            Transform.rotation = newRotation;
            MobileTouchCamera.CamZoom = newZoom;
        }
    }
}

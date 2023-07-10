using AirPlaneSystems;
using UnityEngine;
using Cinemachine;

namespace CameraSystem.Cameras
{
    public class AirPlaneCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AirPlaneController airPlaneController;
        [SerializeField] private CinemachineFreeLook freeLook;
        [Header("Camera values")]
        [SerializeField] private float cameraDefaultFov = 60f;
        [SerializeField] private float cameraTurboFov = 40f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            CameraFovUpdate();
        }

        private void CameraFovUpdate()
        {
            if(!airPlaneController.PlaneIsDead())
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeCameraFov(cameraTurboFov);
                }
                else
                {
                    ChangeCameraFov(cameraDefaultFov);
                }
            }
        }

        private void ChangeCameraFov(float _fov)
        {
            float _deltatime = Time.deltaTime * 100f;
            freeLook.m_Lens.FieldOfView = Mathf.Lerp(freeLook.m_Lens.FieldOfView, _fov, 0.05f * _deltatime);
        }
    }
}
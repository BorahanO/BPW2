using UnityEngine;
using System.Collections;

namespace CGT
{
    [ExecuteInEditMode]
    public class InspectObject : MonoBehaviour
    {
        [Tooltip("Horizontal speed")]
        [SerializeField] private float horizontalSpeed = 5.0F;

        [Tooltip("Vertical speed")]
        [SerializeField] private float verticalSpeed = 5.0F;

        [Tooltip("Zoom speed")]
        public float zoomSpeed = 1;

        [Tooltip("Max and min FOV")]
        public float minFOV = 20;
        public float maxFOV = 75;

        [HideInInspector]
        public Vector3 initialPosition;        

        void Start()
        {                        
            InspectManager.instance.Ping();            
        }

        private void OnEnable()
        {
            if (Application.isPlaying)            
                transform.localEulerAngles = initialPosition;
            
        }

        void Update()
        {
            FirstPersonController.playerCanMove = false;
            FirstPersonController.cameraCanMove = false;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                initialPosition = transform.localEulerAngles;
                if (InspectManager.instance.autoCullingMask)
                    SetLayerRecursively(InspectManager.instance.GetLayerInspect());
            }
#endif 

            float horizontalRotation = horizontalSpeed * Input.GetAxis("Mouse X");
            float verticalRotation = verticalSpeed * Input.GetAxis("Mouse Y");

            if (Mathf.Abs(horizontalRotation) > Mathf.Abs(verticalRotation))
                verticalRotation = 0;
            else
                horizontalRotation = 0;

            if (Input.GetMouseButton(0))
            {
                if(horizontalRotation!=0)
                    transform.Rotate(0, -horizontalRotation, 0, Space.World);
                else
                    transform.Rotate(verticalRotation, 0, 0, Space.World);
            }

            else if (Input.GetMouseButtonDown(1)) 
            {
                transform.localEulerAngles = initialPosition;
                InspectManager.instance.StopInspecting();
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (InspectManager.instance.inspectCamera.fieldOfView < maxFOV)
                    InspectManager.instance.inspectCamera.fieldOfView += zoomSpeed;

            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (InspectManager.instance.inspectCamera.fieldOfView > minFOV)
                    InspectManager.instance.inspectCamera.fieldOfView -= zoomSpeed;
            }
        }

        public void SetLayerRecursively(int layerNumber)
        {
            gameObject.layer = layerNumber;
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CGT
{
    [ExecuteInEditMode]
    public class InspectManager : MonoBehaviour
    {
        public bool blurOnInspect = true;
        public Camera mainCamera;
        public Camera inspectCamera;
        public bool autoCullingMask = true;

        private InspectObject inspecting=null;
        private float initFOV;

        private static string inspectedLayer = "CGT_Inspect";
        private int layerInspect;

        private static InspectManager _instance = null;
        public static InspectManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (InspectManager)FindObjectOfType(typeof(InspectManager));
                    if (_instance == null)
                    {
                        GameObject managers = GameObject.Find("/CGTManagers");
                        if (managers == null)
                            managers = new GameObject("CGTManagers");
                        _instance = (new GameObject("InspectManager")).AddComponent<InspectManager>();
                        _instance.transform.parent = managers.transform;
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {            
#if UNITY_EDITOR
            CreateLayer(inspectedLayer);
#endif
        }

        public bool IsInspecting()
        {
            return inspecting!=null;
        }

        public int GetLayerInspect()
        {
            return layerInspect;
        }

        private void Start()
        {
            if(mainCamera==null)
                mainCamera = Camera.main;

            //If no examine camera set at all, the display a error
            if (inspectCamera == null)
                Debug.LogError("No inspect cam found!! \nPlease set the inspect camera in the script.");
            else
                initFOV = inspectCamera.fieldOfView;

            layerInspect = LayerMask.NameToLayer(inspectedLayer);

            if (Application.isPlaying)
            {
                //Wait for all objects to be set
                StartCoroutine(LateStart(0.1f));

                if (mainCamera.gameObject.GetComponent<CustomOutline>() == null)
                {
                    CustomOutline co = mainCamera.gameObject.AddComponent<CustomOutline>();
                    co.width = 5;
                }
                else
                    mainCamera.gameObject.GetComponent<CustomOutline>().enabled = true;
            }
        }

        private void Update()
        {
            if (autoCullingMask)
            {
                //Set layer mask in main camera
                mainCamera.cullingMask = mainCamera.cullingMask & ~(1 << layerInspect);

                //Set layer mask in inspect camera
                if (inspectCamera != null)
                {
                    inspectCamera.cullingMask = (1 << layerInspect);
                    inspectCamera.clearFlags = CameraClearFlags.Depth;
                }
            }
        }

        IEnumerator LateStart(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }

        public void Ping()
        {

        }

        public void StartInspecting(InspectObject io)
        {
            if (inspecting!=null)
                return;
            inspecting = io;

            inspecting.gameObject.SetActive(true);
            inspectCamera.fieldOfView = initFOV;

            if (blurOnInspect)
            {
                if (mainCamera.gameObject.GetComponent<CustomBlur>() == null)
                {
                    CustomBlur cb = mainCamera.gameObject.AddComponent<CustomBlur>();
                    cb.blurSize = 10;
                }
                else
                    mainCamera.gameObject.GetComponent<CustomBlur>().enabled = true;
            }
        }

        public void StopInspecting()
        {
            if (inspecting==null)
                return;            

            if(mainCamera.gameObject.GetComponent<CustomBlur>()!=null && blurOnInspect)
                mainCamera.gameObject.GetComponent<CustomBlur>().enabled = false;

            inspecting.gameObject.SetActive(false);
            inspecting = null;
        }

#if UNITY_EDITOR
        public static void CreateLayer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentNullException("name", "New layer name string is either null or empty.");

            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layerProps = tagManager.FindProperty("layers");
            var propCount = layerProps.arraySize;

            SerializedProperty firstEmptyProp = null;

            for (var i = 0; i < propCount; i++)
            {
                var layerProp = layerProps.GetArrayElementAtIndex(i);

                var stringValue = layerProp.stringValue;

                if (stringValue == name) return;

                if (i < 8 || stringValue != string.Empty) continue;

                if (firstEmptyProp == null)
                    firstEmptyProp = layerProp;
            }

            if (firstEmptyProp == null)
            {
                UnityEngine.Debug.LogError("Maximum limit of " + propCount + " layers exceeded. Layer \"" + name + "\" not created.");
                return;
            }

            firstEmptyProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }
#endif

    }
}
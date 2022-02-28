using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGT
{

    public class InspectTrigger : MonoBehaviour
    {
        [Tooltip("Object to show when clicked")]
        public InspectObject inspectObject;

        [Tooltip("Outline setup")]
        public bool outline = true;
        public Color outlineColor = Color.red;
        public bool flashing = false;
        [Range(0.5f, 5)]
        public float flashSpeed = 1.5f;
        public float threshold = 1.0f;

        [Tooltip("Label setup")]
        public bool showLabel = true;
        public string Label = "";
        public Font font;
        public int fontSize=25;
        public Color fontColor = Color.white;        
        public bool textBorder = true;
        public Color borderColor = Color.black;


        private bool labelOn=false;
        private GUIStyle guiStyle = new GUIStyle();

        private float timeBlink, deltaRate;

        private Texture2D maskTexture, maskNone;
        private List<Material> materials = new List<Material>();

        private bool ApplyEffect = true;
        private bool currentMode = false;
        private bool blinkMode = false;

        private float andThr;
        private Color outAnt;

        //Simple wrapper to integrate with third party assets
       /*
        public void InspectMe()
        {
            if (inspectObject != null)
                InspectManager.instance.StartInspecting(inspectObject);
        }
        */

        private void Start()
        {
            if(font==null)
                font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        }        

        private void OnMouseEnter()
        {
            if(showLabel)
                labelOn = true;
            if (outline && !InspectManager.instance.IsInspecting())
                ApplyEffect = true;
        }

        private void OnMouseExit()
        {
            labelOn = false;
            ApplyEffect = false;
        }

        private void OnMouseDown()
        {
            if (inspectObject != null)
                InspectManager.instance.StartInspecting(inspectObject);
        }

        void Awake()
        {
            AwakeOutline();
        }

        void OnDisable()
        {
            foreach (Material material in materials)
                material.SetTexture("_SpriteMask", null);
        }

        void Update()
        {
            if (InspectManager.instance.IsInspecting())
                ApplyEffect = false;

            if (flashing && ApplyEffect)
            {
                deltaRate = 1f / (2f * flashSpeed);

                if (Time.time - timeBlink > deltaRate)
                {
                    timeBlink = Time.time;
                    blinkMode = !blinkMode;
                    if (!blinkMode)
                        LightOff();
                    else
                        LightOn();
                }
            }
            else
            {
                if (ApplyEffect)
                    LightOn();
                else
                    LightOff();
            }

            if (andThr != threshold)
            {
                andThr = threshold;
                foreach (Material material in materials)
                    material.SetFloat("_Threshold", threshold);
            }

            if (!outAnt.Equals(outlineColor))
            {
                outlineColor.a = 1.0f;
                outAnt = outlineColor;
                maskTexture.SetPixel(0, 0, outlineColor);
                maskTexture.SetPixel(1, 0, outlineColor);
                maskTexture.SetPixel(0, 1, outlineColor);
                maskTexture.SetPixel(1, 1, outlineColor);
                maskTexture.Apply();
                foreach (Material material in materials)
                    material.SetTexture("_SpriteMask", maskTexture);
            }

        }

        void AwakeOutline()
        {
            materials.Clear();
            maskTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            outlineColor.a = 1.0f;
            maskTexture.SetPixel(0, 0, outlineColor);
            maskTexture.SetPixel(1, 0, outlineColor);
            maskTexture.SetPixel(0, 1, outlineColor);
            maskTexture.SetPixel(1, 1, outlineColor);
            maskTexture.Apply();

            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
                materials.Add(rend.material);

            andThr = threshold;
            outAnt = outlineColor;

            maskNone = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            maskNone.SetPixel(0, 0, new Color(0, 0, 0, 0));
            maskNone.SetPixel(1, 0, new Color(0, 0, 0, 0));
            maskNone.SetPixel(0, 1, new Color(0, 0, 0, 0));
            maskNone.SetPixel(1, 1, new Color(0, 0, 0, 0));
            maskNone.Apply();

            ApplyEffect = false;

        }

        void OnEnable()
        {
            if (ApplyEffect)
            {
                foreach (Material material in materials)
                    material.SetTexture("_SpriteMask", maskTexture);
            }
            else
            {
                foreach (Material material in materials)
                    material.SetTexture("_SpriteMask", maskNone);
            }
            foreach (Material material in materials)
                material.SetFloat("_Threshold", threshold);
        }

        void LightOn()
        {
            if (currentMode)
                return;
            currentMode = true;
            foreach (Material material in materials)
                material.SetTexture("_SpriteMask", maskTexture);
        }

        void LightOff()
        {
            if (!currentMode)
                return;
            currentMode = false;
            foreach (Material material in materials)
                material.SetTexture("_SpriteMask", maskNone);
        }

        void OnGUI()
        {
            if (!labelOn || InspectManager.instance.IsInspecting())
                return;

            guiStyle.font = font;
            guiStyle.fontSize = fontSize;
            guiStyle.normal.textColor= fontColor;

            Vector3 mousePosition = Input.mousePosition;
            float x = mousePosition.x;
            float y = Screen.height - mousePosition.y;
            float width = 400;
            float height = 100;
            Rect rect = new Rect(x+15, y-15, width, height);
            
            if (textBorder)
                DrawOutline(rect, Label, guiStyle, borderColor, fontColor);
            else
                GUI.Label(rect, Label, guiStyle);
        }

        //draw text of a specified color, with a specified outline color
        void DrawOutline(Rect position, string text, GUIStyle theStyle, Color outColor, Color inColor)
        {
            var backupStyle = theStyle;
            theStyle.normal.textColor = outColor;
            position.x--;
            GUI.Label(position, text, theStyle);
            position.x += 2;
            GUI.Label(position, text, theStyle);
            position.x--;
            position.y--;
            GUI.Label(position, text, theStyle);
            position.y += 2;
            GUI.Label(position, text, theStyle);
            position.y--;
            theStyle.normal.textColor = inColor;
            GUI.Label(position, text, theStyle);
            theStyle = backupStyle;
        }
    }
}
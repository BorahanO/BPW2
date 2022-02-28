using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGT
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CustomBlur : MonoBehaviour
    {
        [Range(1,15)]
        public int blurSize = 5;

        //Shader used to get the color depth effect
        Material _shaderMat;
        Material shaderMat
        {
            get
            {
                if (_shaderMat == null)
                {
                    _shaderMat = new Material(Shader.Find("CGT/CustomBlur"));
                    _shaderMat.hideFlags = HideFlags.DontSave;
                }
                return _shaderMat;
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //draws the pixels from the source texture to the destination texture
            shaderMat.SetInt("_BlurSize", blurSize);
            var temporaryTexture = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, destination, shaderMat);
            RenderTexture.ReleaseTemporary(temporaryTexture);
        }

        //Clean buffers before destroy
        void OnDisable()
        {
            if (_shaderMat)
            {
                Material.DestroyImmediate(_shaderMat);
            }
        }
    }
}
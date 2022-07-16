using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Runtime.Scripts.Internal
{
    public class ColorViewer : MonoBehaviour
    {
        [SerializeField]
        private RawImage newColor;
        [SerializeField]
        private RawImage nowColor;
        [SerializeField]
        private Material originalMaterial;

        private static readonly int RGB = Shader.PropertyToID("_RGB");

        private void Awake()
        {
            newColor.material = new Material(originalMaterial);
            nowColor.material = new Material(originalMaterial);
        }

        public void ApplyNewColor(Color color)
        {
            newColor.material.SetColor(RGB, color);
        }

        public void ApplyNowColor(Color color)
        {
            nowColor.material.SetColor(RGB, color);
        }
    }
}

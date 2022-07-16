using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Scripts
{
    public class ColorViewer : MonoBehaviour
    {
        [SerializeField]
        private RawImage newColor;
        [SerializeField]
        private RawImage nowColor;
        [SerializeField]
        private Material originalMaterial;

        private void Awake()
        {
            newColor.material = new Material(originalMaterial);
            nowColor.material = new Material(originalMaterial);
        }

        public void ApplyNewColor(Color color)
        {
            newColor.material.SetColor("_RGB", color);
        }

        public void ApplyNowColor(Color color)
        {
            nowColor.material.SetColor("_RGB", color);
        }
    }
}

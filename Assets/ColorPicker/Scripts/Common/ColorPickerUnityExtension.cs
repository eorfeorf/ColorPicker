using UniRx;
using UnityEngine;

namespace ColorPicker.Scripts.Common
{
    public static class ColorPickerUnityExtension
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Vector3 ToVector3(this Color value)
        {
            return new Vector3(value.r, value.g, value.b);
        }

        public static Vector3 RGBToHSV(this Color value)
        {
            Color.RGBToHSV(value, out var h, out var s, out var v);
            return new Vector3(h, s, v);
        }

        public static Color ToColor(this Vector3 value)
        {
            return Color.HSVToRGB(value.x, value.y, value.z);
        }

        public static Vector3 ToHSV(this Color value)
        {
            return value.RGBToHSV();
        }
    }
}

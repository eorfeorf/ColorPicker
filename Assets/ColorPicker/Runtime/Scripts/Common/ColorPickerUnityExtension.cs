using UnityEngine;

namespace ColorPicker.Runtime.Scripts.Common
{
    public static class ColorPickerUnityExtension
    {
        /// <summary>
        /// float remap.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        /// Color to Vector3.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Color value)
        {
            return new Vector3(value.r, value.g, value.b);
        }

        /// <summary>
        /// RGB to HSV.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 RGBToHSV(this Color value)
        {
            Color.RGBToHSV(value, out var h, out var s, out var v);
            return new Vector3(h, s, v);
        }

        /// <summary>
        /// Vector3 to Color.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color ToColor(this Vector3 value)
        {
            return Color.HSVToRGB(value.x, value.y, value.z);
        }

        /// <summary>
        /// Color to HSV.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 ToHSV(this Color value)
        {
            return value.RGBToHSV();
        }
    }
}

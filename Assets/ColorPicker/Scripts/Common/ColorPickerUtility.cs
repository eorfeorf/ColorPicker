using UnityEngine;

namespace ColorPicker.Scripts
{
    public static class ColorPickerUtility
    {
        public static Vector2 GetLocalPoint(Vector2 pos, RectTransform rectTransform, Camera camera = null)
        {
            camera = camera == null ? Camera.current : camera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, camera, out var localPoint);
            localPoint.x = Mathf.Clamp(localPoint.x, rectTransform.rect.xMin, rectTransform.rect.xMax);
            localPoint.y = Mathf.Clamp(localPoint.y, rectTransform.rect.yMin, rectTransform.rect.yMax);
            return localPoint;
        }

        public static Vector2 GetLocalPoint01(Vector2 localPoint, RectTransform rectTransform)
        {
            // 真ん中が0なので半分を底上げ
            return new Vector2(
                (localPoint.x + Mathf.Abs(rectTransform.rect.xMax)) / rectTransform.rect.width,
                (localPoint.y + Mathf.Abs(rectTransform.rect.yMax)) / rectTransform.rect.height
            );
        }

        /// <summary>
        /// 文字列を0~1に変換.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="result">true:Success false:Fail</param>
        /// <returns></returns>
        public static bool StringTo01(string value, int min, int max, out float result)
        {
            if (int.TryParse(value, out var output))
            {
                // 数値をチェック.
                output = Mathf.Clamp(output, min, max);
                    
                // 0~1に変換.
                var floatOutput = (float)output / max;
                result = floatOutput;
                return true;
            }

            result = 0f;
            return false;
        }

        /// <summary>
        /// 数値文字列をclamp.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ClampString(string text, int min, int max, out string result)
        {
            if (int.TryParse(text, out var output))
            {
                var value = int.Parse(text);
                value = Mathf.Clamp(value, min, max);
                result = value.ToString();
                return true;
            }

            result = text;
            return false;
        }
    }
}
using UnityEngine;

namespace ColorPicker.Scripts.Internal.Common
{
    public static class ColorPickerUtility
    {
        /// <summary>
        /// スクリーン座標から指定したRectTransformのローカル座標に変換.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rectTransform"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector2 GetLocalPoint(Vector2 pos, RectTransform rectTransform, Camera camera = null)
        {
            camera = camera == null ? Camera.current : camera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, camera, out var localPoint);
            localPoint.x = Mathf.Clamp(localPoint.x, rectTransform.rect.xMin, rectTransform.rect.xMax);
            localPoint.y = Mathf.Clamp(localPoint.y, rectTransform.rect.yMin, rectTransform.rect.yMax);
            return localPoint;
        }

        /// <summary>
        /// RectTransformのローカル座標を0~1に変換.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        public static Vector2 LocalPointToUV(Vector2 pos, RectTransform rectTransform)
        {
            Vector2 ret;
            ret.x = pos.x.Remap(rectTransform.rect.xMin, rectTransform.rect.xMax, 0f, 1f);
            ret.y = pos.y.Remap(rectTransform.rect.yMin, rectTransform.rect.yMax, 0f, 1f);
            return ret;
        }
    }
}
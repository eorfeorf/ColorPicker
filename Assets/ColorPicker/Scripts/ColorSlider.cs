using ColorPicker.Scripts.Common;
using UniRx;
using UnityEngine;

namespace ColorPicker.Scripts
{
    public class ColorSlider : MonoBehaviour
    {
        [SerializeField]
        private ColorSliderRect rect;
        [SerializeField]
        private RectTransform pointer;

        public IReactiveProperty<float> Hue01 = new ReactiveProperty<float>();

        private static readonly int HueId = Shader.PropertyToID("_Hue");

        private void Start()
        {
            Observable.Merge(rect.OnPointerClick, rect.OnPointerDrag).Subscribe(data =>
            {
                // ポインタ位置更新.
                var localPoint = GetPointerPosition(data.position, rect.RectTransform, pointer.localPosition);
                pointer.localPosition = localPoint;

                // 真ん中が0なので半分を底上げ.
                Vector2 uv = ColorPickerUtility.GetLocalPoint01(localPoint, rect.RectTransform);

                Debug.Log($"ColorSlider: ClickPosition={localPoint}");
                Debug.Log($"ColorSlider: UV={uv}");
                
                // 色相に適用.
                Hue01.Value = uv.y;
            }).AddTo(this);

            // 初期位置.
            pointer.localPosition = new Vector3(rect.RectTransform.rect.x, rect.RectTransform.rect.yMax, pointer.localPosition.z);
        }

        private Vector2 GetPointerPosition(Vector2 screenPos, RectTransform rectRectTransform, Vector3 pointerLocalPosition)
        {
            var localPoint = ColorPickerUtility.GetLocalPoint(screenPos, rectRectTransform);
            localPoint.y = Mathf.Clamp(localPoint.y, rectRectTransform.rect.yMin, rectRectTransform.rect.yMax);
            localPoint.x = pointerLocalPosition.x;
            return localPoint;
        }

        public void Apply(float hue)
        {
            var rc = rect.RectTransform.rect;
            var height = hue.Remap(0f, 1f, rc.yMin, rc.yMax);
            pointer.localPosition = new Vector3(pointer.localPosition.x, height, pointer.localPosition.z);
        }
    }
}

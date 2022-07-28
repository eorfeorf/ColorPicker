using ColorPicker.Scripts.Internal.Common;
using UniRx;
using UnityEngine;

namespace ColorPicker.Scripts.Internal
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
                var localPoint = ColorPickerUtility.GetLocalPoint(data.position, rect.RectTransform);
                Debug.Log($"ColorSlider: ClickPosition={localPoint}");
                localPoint.x = pointer.localPosition.x;
                pointer.localPosition = localPoint;

                // 座標を0~1にしてHueに適用.
                Vector2 uv = ColorPickerUtility.LocalPointToUV(localPoint, rect.RectTransform);
                Debug.Log($"ColorSlider: UV={uv}");
                Hue01.Value = uv.y;
            }).AddTo(this);

            // 初期位置.
            pointer.localPosition = new Vector3(rect.RectTransform.rect.x, rect.RectTransform.rect.yMax, pointer.localPosition.z);
        }

        public void Apply(float hue)
        {
            var rc = rect.RectTransform.rect;
            var height = hue.Remap(0f, 1f, rc.yMin, rc.yMax);
            pointer.localPosition = new Vector3(pointer.localPosition.x, height, pointer.localPosition.z);
        }
    }
}

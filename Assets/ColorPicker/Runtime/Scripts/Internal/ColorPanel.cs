using System;
using ColorPicker.Scripts.Common;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColorPicker.Scripts
{
    public class ColorPanel : MonoBehaviour
    {
        [SerializeField]
        private ColorPanelRect rect;
        [SerializeField]
        private RectTransform pointer;

        public IReadOnlyReactiveProperty<Vector2> SV01 => sv;
        private ReactiveProperty<Vector2> sv = new ReactiveProperty<Vector2>();

        private static readonly int Hue = Shader.PropertyToID("_Hue");
        
        private void Start()
        {
            Observable.Merge(rect.OnPointerClick, rect.OnPointerDrag).Subscribe(data =>
            {
                // ポインタ位置更新.
                var localPoint = ColorPickerUtility.GetLocalPoint(data.position, rect.RectTransform);
                Debug.Log($"ColorPanel: ClickPosition={localPoint}");
                pointer.localPosition = localPoint;
                 
                // 座標を0~1にしてSaturation, Valueに適用. 
                var uv = ColorPickerUtility.LocalPointToUV(localPoint, rect.RectTransform);
                Debug.Log($"ColorPanel: UV={uv}");
                sv.Value = uv;
            }).AddTo(this);
        }

        public void Apply(Vector3 hsv)
        {
            ApplyRectMaterial(hsv.x);
            ApplyPointerPosition(hsv);
        }

        private void ApplyRectMaterial(float h)
        {
            rect.Material.SetFloat(Hue, h);
        }

        private void ApplyPointerPosition(Vector3 hsv)
        {
            var x = hsv.y;
            var y = hsv.z;
            var rc = rect.RectTransform.rect;
            x = x.Remap(0f, 1f, rc.xMin, rc.xMax);
            y = y.Remap(0f, 1f, rc.yMin, rc.yMax);
            pointer.localPosition = new Vector3(x, y, pointer.localPosition.z);
        }
    }
}
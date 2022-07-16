using TMPro;
using UniRx;
using UnityEngine;

namespace ColorPicker.Scripts
{
    /// <summary>
    /// HSV数値表示クラス.
    /// H : 0 ~ 360 °
    /// S : 0 ~ 100 %
    /// V : 0 ~ 100 %
    /// </summary>
    public class ParameterHSV : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputFieldH;
        [SerializeField]
        private TMP_InputField inputFieldS;
        [SerializeField]
        private TMP_InputField inputFieldV;

        public IReactiveProperty<float> OnEditH => onEditH;
        private ReactiveProperty<float> onEditH = new ReactiveProperty<float>();
        public IReactiveProperty<float> OnEditS => onEditS;
        private ReactiveProperty<float> onEditS = new ReactiveProperty<float>();
        public IReactiveProperty<float> OnEditV => onEditV;
        private ReactiveProperty<float> onEditV = new ReactiveProperty<float>();

        private void Start()
        {
            Initialize(inputFieldH, onEditH, ColorPickerDefine.H_MIN, ColorPickerDefine.H_MAX);
            Initialize(inputFieldS, onEditS, ColorPickerDefine.S_MIN, ColorPickerDefine.S_MAX);
            Initialize(inputFieldV, onEditV, ColorPickerDefine.V_MIN, ColorPickerDefine.V_MAX);
        }

        private void Initialize(TMP_InputField inputField, ReactiveProperty<float> onEdit, int min, int max)
        {
            onEdit.Subscribe(value =>
            {
                value = Mathf.Clamp01(value);
                // 文字列に反映.
                var intValue = value * max;
                inputField.SetTextWithoutNotify(intValue.ToString());
            }).AddTo(this);
            
            // TODO:数値が空の時にonEndEditが呼ばれた場合０を入れるようにする
            Observable.Merge(inputField.onEndEdit.AsObservable(), inputField.onValueChanged.AsObservable()).Subscribe(value =>
            {
                // 数値か？.
                if(int.TryParse(value, out var intValue))
                {
                    intValue = Mathf.Clamp(intValue, min, max);
                    // 0~1.
                    var value01 = (float)intValue / max;
                    onEdit.SetValueAndForceNotify(value01);
                }
                else
                {
                    Debug.LogWarning("ParameterRGB : Invalid parameter.");
                }
            }).AddTo(this);
        }
        
        /// <summary>
        /// 一括適用.
        /// </summary>
        /// <param name="rgb"></param>
        public void Apply(Vector3 hsv)
        {
            ApplyH(hsv.x);
            ApplyS(hsv.y);
            ApplyV(hsv.z);
        }

        /// <summary>
        /// H適用.
        /// </summary>
        /// <param name="h"></param>
        private void ApplyH(float h)
        {
            ApplyText(h, inputFieldH, ColorPickerDefine.H_MAX);
        }

        /// <summary>
        /// S適用.
        /// </summary>
        /// <param name="s"></param>
        private void ApplyS(float s)
        {
            ApplyText(s, inputFieldS, ColorPickerDefine.S_MAX);
        }
        
        /// <summary>
        /// V適用.
        /// </summary>
        /// <param name="v"></param>
        private void ApplyV(float v)
        {
            ApplyText(v, inputFieldV, ColorPickerDefine.V_MAX);
        }
        
        private void ApplyText(float value, TMP_InputField inputField, int max)
        {
            value = Mathf.Clamp01(value);
            // 文字列に反映.
            var intValue = (int)(value * max);
            inputField.SetTextWithoutNotify(intValue.ToString());
        }
    }
}

using TMPro;
using UniRx;
using UnityEngine;

namespace ColorPicker.Scripts
{
    public class ParameterRGB : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputFieldR;
        [SerializeField]
        private TMP_InputField inputFieldG;
        [SerializeField]
        private TMP_InputField inputFieldB;

        public IReadOnlyReactiveProperty<float> OnEditR => onEditR;
        private ReactiveProperty<float> onEditR = new ReactiveProperty<float>();
        public ReactiveProperty<float> OnEditG => onEditG;
        private ReactiveProperty<float> onEditG = new ReactiveProperty<float>();
        public ReactiveProperty<float> OnEditB => onEditB;
        private ReactiveProperty<float> onEditB = new ReactiveProperty<float>();
        
        private void Start()
        {
            Initialize(inputFieldR, onEditR, ColorPickerDefine.RGB_MIN, ColorPickerDefine.RGB_MAX);
            Initialize(inputFieldG, OnEditG, ColorPickerDefine.RGB_MIN, ColorPickerDefine.RGB_MAX);
            Initialize(inputFieldB, OnEditB, ColorPickerDefine.RGB_MIN, ColorPickerDefine.RGB_MAX);
        }

        private void Initialize(TMP_InputField inputField, ReactiveProperty<float> onEdit, int min, int max)
        {
            Observable.Merge(inputField.onEndEdit.AsObservable(), inputField.onValueChanged.AsObservable()).Subscribe(value =>
            {
                // 数値か？.
                if(int.TryParse(value, out var intValue))
                {
                    intValue = Mathf.Clamp(intValue, min, max);
                    // Clampした結果を文字に反映.
                    inputField.SetTextWithoutNotify(intValue.ToString());
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
        /// <param name="color"></param>
        public void Apply(Color color)
        {
            ApplyR(color.r);
            ApplyG(color.g);
            ApplyB(color.b);
        }
        
        /// <summary>
        /// R適用.
        /// </summary>
        /// <param name="r"></param>
        private void ApplyR(float r)
        {
            ApplyText(r, inputFieldR, ColorPickerDefine.RGB_MAX);
        }

        /// <summary>
        /// G適用.
        /// </summary>
        /// <param name="g"></param>
        private void ApplyG(float g)
        {
            ApplyText(g, inputFieldG, ColorPickerDefine.RGB_MAX);
        }
        
        /// <summary>
        /// B適用
        /// </summary>
        /// <param name="b"></param>
        private void ApplyB(float b)
        {
            ApplyText(b, inputFieldB, ColorPickerDefine.RGB_MAX);
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

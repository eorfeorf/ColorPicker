using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace ColorPicker.Runtime.Scripts.Internal
{
    public class ParameterCode : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputField;

        public IReadOnlyReactiveProperty<Color> OnEditCode => onEditCode;
        private ReactiveProperty<Color> onEditCode = new ReactiveProperty<Color>();
        
        private void Start()
        {
            Observable.Merge(inputField.onEndEdit.AsObservable(), inputField.onValueChanged.AsObservable()).Subscribe(value =>
            {
                // カラーコードか？.
                value = value.Insert(0, "#");
                if(ColorUtility.TryParseHtmlString(value, out var color))
                {
                    onEditCode.SetValueAndForceNotify(color);
                }
                else
                {
                    Debug.LogWarning("ParameterCode : Invalid parameter.");
                }
            }).AddTo(this);
        }

        public void Apply(Color color)
        {
            var code =ColorUtility.ToHtmlStringRGB(color);
            inputField.SetTextWithoutNotify(code);
        }
    }
}

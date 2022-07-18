using ColorPicker.Runtime.Scripts.Common;
using ColorPicker.Runtime.Scripts.Internal;
using UniRx;
using UnityEngine;

namespace ColorPicker.Runtime.Scripts
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField]
        private Buttons buttons;
        [SerializeField]
        private ColorViewer colorViewer;
        [SerializeField]
        private ColorPanel colorPanel;
        [SerializeField]
        private ColorSlider colorSlider;
        [SerializeField]
        private ParameterRGB parameterRGB;
        [SerializeField]
        private ParameterHSV parameterHSV;
        [SerializeField]
        private ParameterCode parameterCode;

        /// <summary>
        /// 閉じるボタン.
        /// </summary>
        public IReadOnlyReactiveProperty<(Color newColor, Color nowColor)> OnCloseButton => onCloseButton;
        private ReactiveProperty<(Color newColor, Color nowColor)> onCloseButton = new ReactiveProperty<(Color, Color)>();
        /// <summary>
        /// Saveボタン.
        /// </summary>
        public IReadOnlyReactiveProperty<Color> OnSaveButton => onSaveButton;
        private ReactiveProperty<Color> onSaveButton = new ReactiveProperty<Color>();
        /// <summary>
        /// Cancelボタン.
        /// </summary>
        public IReadOnlyReactiveProperty<Color> OnCancelButton => onCancelButton;
        private ReactiveProperty<Color> onCancelButton = new ReactiveProperty<Color>();
        /// <summary>
        /// 色変更中.
        /// </summary>
        public IReadOnlyReactiveProperty<Color> OnChanged => onChanged;
        private ReactiveProperty<Color> onChanged = new ReactiveProperty<Color>();
        
        /// <summary>
        /// 現在の色.
        /// </summary>
        private Vector3 hsv = Vector3.one;
        /// <summary>
        /// 保存される前の色.
        /// </summary>
        private Vector3 prevHsv = Vector3.one;
        /// <summary>
        /// すでに開いているか？.
        /// </summary>
        private bool isOpend = false;
        /// <summary>
        /// すでに閉じているか？
        /// </summary>
        private bool isClosed = false;
        
        private void Start()
        {
            // Buttons
            buttons.OnClose.Subscribe(_ =>
            {
                onCloseButton.SetValueAndForceNotify((hsv.ToColor(), prevHsv.ToColor()));
                prevHsv = hsv;
                CloseAction();
            }).AddTo(this);
            buttons.OnSave.Subscribe(_ =>
            {
                onSaveButton.SetValueAndForceNotify(hsv.ToColor());
                prevHsv = hsv;  
                CloseAction();
            }).AddTo(this);
            buttons.OnCancel.Subscribe(_ =>
            {
                onCancelButton.SetValueAndForceNotify(prevHsv.ToColor());
                hsv = prevHsv;
                CloseAction();
            }).AddTo(this);
            
            // ColorViewer(イベント無し)
            
            // ColorPanel 
            colorPanel.SV01.Subscribe(sv =>
            {
                hsv.y = sv.x;
                hsv.z = sv.y;
                ApplyOnChangedColorPanel(hsv);
            }).AddTo(this);

            // ColorSlider
            colorSlider.Hue01.Subscribe(hue =>
            {
                hsv.x = hue;
                ApplyOnChangedColorSlider(hsv);
            }).AddTo(this);
            
            // ParameterRGB
            parameterRGB.OnEditR.Subscribe(value =>
            {
                var rgb = hsv.ToColor();
                rgb.r = value;
                hsv = rgb.ToHSV();
                ApplyOnChangedParameterRGB(rgb);
            }).AddTo(this);
            parameterRGB.OnEditG.Subscribe(value =>
            {
                var rgb = hsv.ToColor();
                rgb.g = value;
                hsv = rgb.ToHSV();
                ApplyOnChangedParameterRGB(rgb);
            }).AddTo(this);
            parameterRGB.OnEditB.Subscribe(value =>
            {
                var rgb = hsv.ToColor();
                rgb.b = value;
                hsv = rgb.ToHSV();
                ApplyOnChangedParameterRGB(rgb);
            }).AddTo(this);

            // ParameterHSV
            parameterHSV.OnEditH.Subscribe(value =>
            {
                hsv.x = value;
                ApplyOnChangedParameterHSV(hsv);
            }).AddTo(this);
            parameterHSV.OnEditS.Subscribe(value =>
            {
                hsv.y = value;
                ApplyOnChangedParameterHSV(hsv);
            }).AddTo(this);
            parameterHSV.OnEditV.Subscribe(value =>
            {
                hsv.z = value;
                ApplyOnChangedParameterHSV(hsv);
            }).AddTo(this);

            // ParameterCode.
            parameterCode.OnEditCode.Subscribe(value =>
            {
                hsv = value.ToHSV();
                ApplyOnChangedParameterCode(hsv);
            }).AddTo(this);
        }

        #region Apply
        /// <summary>
        /// 色変更時.
        /// </summary>
        /// <param name="hsv"></param>
        private void SetOnChangedColor(Vector3 hsv)
        {
            onChanged.Value = hsv.ToColor();
        }
        
        /// <summary>
        /// ColorPanel変更時の適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedColorPanel(Vector3 hsv)
        {
            var color = hsv.ToColor();
            SetOnChangedColor(hsv);
            colorViewer.ApplyNewColor(color);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
            parameterCode.Apply(color);
        }

        /// <summary>
        /// ColorSlider変更時の適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedColorSlider(Vector3 hsv)
        {
            var color = hsv.ToColor();
            SetOnChangedColor(hsv);
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
            parameterCode.Apply(color);
        }
        
        /// <summary>
        /// ParameterRGB変更による適用.
        /// </summary>
        /// <param name="color"></param>
        private void ApplyOnChangedParameterRGB(Color color)
        {
            var hsv = color.RGBToHSV();
            SetOnChangedColor(hsv);
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterHSV.Apply(hsv);
            parameterCode.Apply(color);
        }

        /// <summary>
        /// ParameterHSV変更による適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedParameterHSV(Vector3 hsv)
        {
            var color = hsv.ToColor();
            SetOnChangedColor(hsv);
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterRGB.Apply(color);
            parameterCode.Apply(color);
        }

        /// <summary>
        /// ParameterCode変更による適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedParameterCode(Vector3 hsv)
        {
            var color = hsv.ToColor();
            SetOnChangedColor(hsv);
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
        }
        
        #endregion

        /// <summary>
        /// 閉じる時の必須処理.
        /// </summary>
        private void CloseAction()
        {
            isOpend = false;
            isClosed = true;
            gameObject.SetActive(false);
        }

        #region public
        public void Open(Color? nowColor = null)
        {
            // 二重オープンはしない.
            if (isOpend)
            {
                return;
            }
            isOpend = true;
            isClosed = false;
            gameObject.SetActive(true);
            
            if (nowColor != null)
            {
                prevHsv = nowColor.Value.ToHSV();
            }
            hsv = prevHsv;
            colorViewer.ApplyNowColor(hsv.ToColor());
            colorViewer.ApplyNewColor(hsv.ToColor());
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterRGB.Apply(hsv.ToColor());
            parameterHSV.Apply(hsv);
        }

        public void Close()
        {
            if (isClosed)
            {
                return;
            }
            CloseAction();
        }
        #endregion
    }
}

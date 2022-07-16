using ColorPicker.Scripts.Common;
using UniRx;
using UnityEngine;

namespace ColorPicker.Scripts
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
        /// 開く.
        /// </summary>
        public IReadOnlyReactiveProperty<Color> OnOpen => onOpen;
        private ReactiveProperty<Color> onOpen = new ReactiveProperty<Color>();
        /// <summary>
        /// 閉じる.
        /// </summary>
        public IReadOnlyReactiveProperty<(Color newColor, Color nowColor)> OnClose => onClose;
        private ReactiveProperty<(Color newColor, Color nowColor)> onClose = new ReactiveProperty<(Color newColor, Color nowColor)>();

        
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
        
        private void Start()
        {
            #region Buttons
            buttons.OnClose.Subscribe(_ =>
            {
                onCloseButton.SetValueAndForceNotify((hsv.ToColor(), prevHsv.ToColor()));
                prevHsv = hsv;
                Close();
            }).AddTo(this);
            buttons.OnSave.Subscribe(_ =>
            {
                onSaveButton.SetValueAndForceNotify(hsv.ToColor());
                prevHsv = hsv;  
                Close();
            }).AddTo(this);
            buttons.OnCancel.Subscribe(_ =>
            {
                onCancelButton.SetValueAndForceNotify(prevHsv.ToColor());
                hsv = prevHsv;
                Close();
            }).AddTo(this);
            #endregion
            
            #region  ParameterRGB
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
            #endregion

            #region ParameterHSV
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
            #endregion

            #region ColorPanel 
            colorPanel.SV01.Subscribe(sv =>
            {
                hsv.y = sv.x;
                hsv.z = sv.y;
                ApplyOnChangedColorPanel(hsv);
            }).AddTo(this);
            #endregion

            #region ColorSlider
            colorSlider.Hue01.Subscribe(hue =>
            {
                hsv.x = hue;
                ApplyOnChangedColorSlider(hsv);
            }).AddTo(this);
            #endregion
            
            //
            // ColorViewer(イベント無し)
            //
            
        }

        #region Apply
        private void ChangeColor(Vector3 hsv)
        {
            onChanged.Value = hsv.ToColor();
        }

        private void ApplyOnChangedParameterRGB(Color color)
        {
            var hsv = color.RGBToHSV();
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterHSV.Apply(hsv);
            ChangeColor(hsv);
        }

        private void ApplyOnChangedParameterHSV(Vector3 hsv)
        {
            var color = hsv.ToColor();
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterRGB.Apply(color);
            ChangeColor(hsv);
        }

        private void ApplyOnChangedColorPanel(Vector3 hsv)
        {
            var color = hsv.ToColor();
            colorViewer.ApplyNewColor(color);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
            ChangeColor(hsv);
        }

        private void ApplyOnChangedColorSlider(Vector3 hsv)
        {
            var color = hsv.ToColor();
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
            ChangeColor(hsv);
        }
        #endregion

        #region public
        public void Open(Color? nowColor = null)
        {
            if (isOpend)
            {
                return;
            }
            isOpend = true;
            
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

            onOpen.SetValueAndForceNotify(hsv.ToColor());
        }

        public void Close()
        {
            isOpend = false;
            gameObject.SetActive(false);
            
            onClose.SetValueAndForceNotify((hsv.ToColor(), prevHsv.ToColor()));
        }
        #endregion
    }
}

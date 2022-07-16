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
            
            // ColorViewer(イベント無し)
        }

        #region Apply
        /// <summary>
        /// ParameterRGB変更による適用.
        /// </summary>
        /// <param name="color"></param>
        private void ApplyOnChangedParameterRGB(Color color)
        {
            var hsv = color.RGBToHSV();
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterHSV.Apply(hsv);
            SetOnChangedColor(hsv);
        }

        /// <summary>
        /// ParameterHSV変更による適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedParameterHSV(Vector3 hsv)
        {
            var color = hsv.ToColor();
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            colorSlider.Apply(hsv.x);
            parameterRGB.Apply(color);
            SetOnChangedColor(hsv);
        }

        /// <summary>
        /// ColorPanel変更時の適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedColorPanel(Vector3 hsv)
        {
            var color = hsv.ToColor();
            colorViewer.ApplyNewColor(color);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
            SetOnChangedColor(hsv);
        }

        /// <summary>
        /// ColorSlider変更時の適用.
        /// </summary>
        /// <param name="hsv"></param>
        private void ApplyOnChangedColorSlider(Vector3 hsv)
        {
            var color = hsv.ToColor();
            colorViewer.ApplyNewColor(color);
            colorPanel.Apply(hsv);
            parameterRGB.Apply(color);
            parameterHSV.Apply(hsv);
            SetOnChangedColor(hsv);
        }
        /// <summary>
        /// 色変更時.
        /// </summary>
        /// <param name="hsv"></param>
        private void SetOnChangedColor(Vector3 hsv)
        {
            onChanged.Value = hsv.ToColor();
        }
        
        #endregion

        /// <summary>
        /// 閉じる時の必須処理.
        /// </summary>
        private void CloseAction()
        {
            isOpend = false;
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
            onClose.SetValueAndForceNotify((hsv.ToColor(), prevHsv.ToColor()));
            CloseAction();
        }
        #endregion
    }
}

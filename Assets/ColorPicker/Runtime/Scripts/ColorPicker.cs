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

        
        public IReadOnlyReactiveProperty<(Color newColor, Color nowColor)> OnCloseButton => onCloseButton;
        private ReactiveProperty<(Color newColor, Color nowColor)> onCloseButton = new ReactiveProperty<(Color, Color)>();
        public IReadOnlyReactiveProperty<Color> OnSaveButton => onSaveButton;
        private ReactiveProperty<Color> onSaveButton = new ReactiveProperty<Color>();
        public IReadOnlyReactiveProperty<Color> OnCancelButton => onCancelButton;
        private ReactiveProperty<Color> onCancelButton = new ReactiveProperty<Color>();
        public IReadOnlyReactiveProperty<Color> OnChanged => onChanged;
        private ReactiveProperty<Color> onChanged = new ReactiveProperty<Color>();

        private Vector3 hsv = Vector3.one;
        private Vector3 prevHsv = Vector3.one;
        
        private void Start()
        {
            //
            // Buttons
            //
            buttons.OnClose.Subscribe(_ =>
            {
                onCloseButton.SetValueAndForceNotify((hsv.ToColor(), prevHsv.ToColor()));
            }).AddTo(this);
            buttons.OnSave.Subscribe(_ =>
            {
                onSaveButton.SetValueAndForceNotify(hsv.ToColor());
            }).AddTo(this);
            buttons.OnCancel.Subscribe(_ =>
            {
                onCancelButton.SetValueAndForceNotify(prevHsv.ToColor());
            }).AddTo(this);
            
            //
            // ParameterRGB
            //
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
            
            //
            // ParameterHSV
            //
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
            
            //
            // ColorPanel
            //
            colorPanel.SV01.Subscribe(sv =>
            {
                hsv.y = sv.x;
                hsv.z = sv.y;
                ApplyOnChangedColorPanel(hsv);
            }).AddTo(this);
            
            //
            // ColorSlider
            //
            colorSlider.Hue01.Subscribe(hue =>
            {
                hsv.x = hue;
                ApplyOnChangedColorSlider(hsv);
            }).AddTo(this);
            
            //
            // ColorViewer(イベント無し)
            //
            
        }

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

        public void OnEnable()
        {
            prevHsv = hsv;
        }
    }
}

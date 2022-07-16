using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Examples
{
    public class Examples : MonoBehaviour
    {
        [SerializeField]
        private Scripts.ColorPicker colorPicker;

        [SerializeField]
        private RawImage image;

        private void Start()
        {
            // 変更中.
            colorPicker.OnChanged.Subscribe(changedColor =>
            {
                image.color = changedColor;
            }).AddTo(this);

            // セーブボタン.
            colorPicker.OnSaveButton.Subscribe(fixColor =>
            {
                image.color = fixColor;
            }).AddTo(this);
            
            // キャンセルボタン.
            colorPicker.OnCancelButton.Subscribe(oldColor =>
            {
                image.color = oldColor;
            }).AddTo(this);
            
            // 閉じるボタン.
            colorPicker.OnCloseButton.Subscribe(colors =>
            {
                image.color = colors.newColor;
            }).AddTo(this);
            
            colorPicker.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                colorPicker.Open();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                var colors = colorPicker.Close();
                image.color = colors.nowColor;
            }
        }
    }
}

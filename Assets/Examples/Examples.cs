using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Examples
{
    public class Examples : MonoBehaviour
    {
        [SerializeField]
        private ColorPicker.Scripts.ColorPicker colorPicker;

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
            colorPicker.OnSaveButton.Subscribe(newColor =>
            {
                image.color = newColor;
            }).AddTo(this);
            
            // キャンセルボタン.
            colorPicker.OnCancelButton.Subscribe(nowColor =>
            {
                image.color = nowColor;
            }).AddTo(this);
            
            // 閉じるボタン.
            colorPicker.OnCloseButton.Subscribe(colors =>
            {
                image.color = colors.newColor;
                //image.color = colors.nowColor;
            }).AddTo(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                colorPicker.Open();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                colorPicker.Close();
            }
        }
    }
}

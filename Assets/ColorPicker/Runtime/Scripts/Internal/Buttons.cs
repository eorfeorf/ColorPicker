using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Scripts
{
    public class Buttons : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button cancelButton;

        public IObservable<Unit> OnClose { get; private set; }
        public IObservable<Unit> OnSave { get; private set; }
        public IObservable<Unit> OnCancel { get; private set; }

        private void Start()
        {
            OnClose = closeButton.OnClickAsObservable();
            OnSave = saveButton.OnClickAsObservable();
            OnCancel = cancelButton.OnClickAsObservable();
        }
    }
}

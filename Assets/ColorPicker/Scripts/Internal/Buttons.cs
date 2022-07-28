using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Scripts.Internal
{
    public class Buttons : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private Button saveButton;
        [SerializeField]
        private Button cancelButton;

        public IObservable<Unit> OnClose => onClose;
        public Subject<Unit> onClose = new Subject<Unit>();
        public IObservable<Unit> OnSave => onSave;
        public Subject<Unit> onSave = new Subject<Unit>();
        public IObservable<Unit> OnCancel => onCancel;
        public Subject<Unit> onCancel = new Subject<Unit>();

        private void Start()
        {
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                onClose.OnNext(Unit.Default);    
            }).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ =>
            {
                onSave.OnNext(Unit.Default);
            }).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ =>
            {
                onCancel.OnNext(Unit.Default);
            }).AddTo(this);
        }
    }
}

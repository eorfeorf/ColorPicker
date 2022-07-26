using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Runtime.Scripts.Internal
{
    public class EyeDropperView : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public IObservable<Unit> OnClickObservable => button.OnClickAsObservable();
    }
}

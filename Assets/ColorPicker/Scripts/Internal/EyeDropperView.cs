using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPicker.Scripts.Internal
{
    public class EyeDropperView : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public IObservable<Unit> OnClickObservable => button.OnClickAsObservable();
    }
}

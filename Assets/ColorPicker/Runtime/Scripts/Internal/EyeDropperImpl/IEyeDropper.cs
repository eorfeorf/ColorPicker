using System;
using UniRx;
using UnityEngine;

namespace ColorPicker.Runtime.Scripts.Internal.EyeDropperImpl
{
    public interface IEyeDropper : IDisposable
    {
        public IReadOnlyReactiveProperty<Color> OnChangeColor { get; }
        public IReadOnlyReactiveProperty<Color> OnClickColor { get; }
        public abstract void Ready();
        public abstract void Unready();
    }
}


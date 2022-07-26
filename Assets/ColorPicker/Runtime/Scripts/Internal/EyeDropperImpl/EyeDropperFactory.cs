using UnityEngine;

namespace ColorPicker.Runtime.Scripts.Internal.EyeDropperImpl
{
    public static class EyeDropperFactory
    {
        public static IEyeDropper Create()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            return new EyeDropperWindows();
#endif
            return null;
        }
    }
}

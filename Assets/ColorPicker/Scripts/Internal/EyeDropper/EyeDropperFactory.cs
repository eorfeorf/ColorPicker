namespace ColorPicker.Scripts.Internal.EyeDropper
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

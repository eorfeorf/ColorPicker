using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Codice.Client.ChangeTrackerService;
using UniRx;
using UnityEngine;
namespace ColorPicker.Scripts.Internal.EyeDropper
{
    /// <summary>
    /// スポイト　Windows実装.
    /// </summary>
    public class EyeDropperWindows : IEyeDropper
    {

        /// <summary>
        /// 変更中.
        /// </summary>
        public IReadOnlyReactiveProperty<Color> OnChangeColor => onChangeColor;
        private ReactiveProperty<Color> onChangeColor = new ReactiveProperty<Color>();
        /// <summary>
        /// クリック時.
        /// </summary>
        public IReadOnlyReactiveProperty<Color> OnClickColor => onClickColor;
        private static ReactiveProperty<Color> onClickColor = new ReactiveProperty<Color>();

        #region Win32API
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 513;
        
        private delegate IntPtr DelegateHookCallback(int nCode, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, DelegateHookCallback lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out Win32Api.POINT point);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        private DelegateHookCallback hookProc = null;
        private IntPtr hookPtr = IntPtr.Zero;
        private IntPtr hdc;
        #endregion

        /// <summary>
        /// 取得可能状態か？
        /// </summary>
        private static bool isGettingState = false;
        /// <summary>
        /// クリック時にGetPixel()を呼ぶと正常な値が帰ってこないため、色をキャッシュしておく. 
        /// </summary>
        private static Color selectColor = Color.white;
        private CompositeDisposable disposable = new CompositeDisposable();

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public EyeDropperWindows()
        {
            // このアプリケーションに対して処理をフックする.
            hdc = GetDC(IntPtr.Zero);
            using var currentProcess = Process.GetCurrentProcess();
            using var curModule = currentProcess.MainModule;
            hookProc = HookCallback;
            hookPtr = SetWindowsHookEx(
                WH_MOUSE_LL,
                HookCallback,
                GetModuleHandle(curModule.ModuleName),
                0
            );

            // Updateで色を取得する.
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (!isGettingState) return;
                selectColor = GetColor();
                onChangeColor.SetValueAndForceNotify(selectColor);
            }).AddTo(disposable);
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            hookProc = null;
            UnhookWindowsHookEx(hookPtr);
            hookPtr = IntPtr.Zero;
            ReleaseDC(IntPtr.Zero, hdc);
            disposable.Dispose();
        }

        /// <summary>
        /// 取得可能状態に変更.
        /// </summary>
        public void Ready()
        {
            isGettingState = true;
        }

        /// <summary>
        /// 取得不可状態に変更.
        /// </summary>
        public void Unready()
        {
            isGettingState = false;
        }
        
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (isGettingState)
            {
                if (nCode >= 0 && wParam == (IntPtr) WM_LBUTTONDOWN)
                {
                    onClickColor.SetValueAndForceNotify(selectColor);
                    // 入力を捨てる.
                    return new IntPtr(1);
                }
            }
            
            // 入力をスルー.
            return new IntPtr(0);
        }
        
        /// <summary>
        /// マウスの座標にある色を取得.
        /// </summary>
        /// <returns></returns>
        private Color GetColor()
        {
            GetCursorPos(out var point);
            var pixel = GetPixel(hdc, point.X, point.Y);
            int r = (int)(pixel & 0x000000FF);
            int g = (int)(pixel & 0x0000FF00) >> 8;
            int b = (int)(pixel & 0x00FF0000) >> 16;
            Color ret = new Color(
                (float)r / ColorPickerDefine.RGB_MAX,
                (float)g / ColorPickerDefine.RGB_MAX,
                (float)b / ColorPickerDefine.RGB_MAX);
            return ret;
        }
    }
}


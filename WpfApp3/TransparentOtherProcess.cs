using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace HaruaConvert
{
    public class TransparentOtherProcess
    {
        // そのウィンドウハンドルをもつウィンドウは存在するか？ 存在するなら0以外の値を返す
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int IsWindow(IntPtr hWnd);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);
        const uint GA_PARENT = 1;
        const uint GA_ROOT = 2;
        const uint GA_ROOTOWNER = 3;

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);


        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x00080000;

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        private IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        private IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
        }


        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


        public bool AddExStyle(IntPtr handle, int style)
        {
            int winFlags = (int)GetWindowLongPtr(handle, GWL_EXSTYLE);

            if (winFlags == 0)
            {
                int error = Marshal.GetLastWin32Error();
                if (error != 0)
                {
                    MessageBox.Show("GetWindowLongPtrが失敗");
                    return false;
                }
            }

            if ((winFlags & WS_EX_LAYERED) == 0)
            {
                winFlags |= WS_EX_LAYERED;
                SetWindowLongPtr(handle, GWL_EXSTYLE, new IntPtr(winFlags));
            }
            return true;
        }


        public bool SetTransParentProcess(IntPtr handle, byte alpha)
        {
            IntPtr rootHandle = GetAncestor(handle, GA_ROOT);


            //// WS_EX_LAYEREDがないなら追加する
            //if (!AddExStyle(rootHandle, WS_EX_LAYERED))
            //    return false;
            if (handle != (IntPtr)0x00000000)
            {


                if (!SetLayeredWindowAttributes(handle, 0, alpha, 0x2))
                {
                    MessageBox.Show("SetLayeredWindowAttributesが失敗");
                    return false;
                }
            }


            return true;
        }
    }
}

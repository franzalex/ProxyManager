using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProxyManager
{
    class WinApi
    {
        // http://www.vbaccelerator.com/home/NET/Code/Controls/ListBox_and_ComboBox/TextBox_Icon/article.asp
        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int SendMessage(
           IntPtr hwnd,
           int wMsg,
           int wParam,
           int lParam);

        private const int EC_LEFTMARGIN = 0x1;
        private const int EC_RIGHTMARGIN = 0x2;
        private const int EC_USEFONTINFO = 0xFFFF;
        private const int EM_SETMARGINS = 0xD3;
        private const int EM_GETMARGINS = 0xD4;


        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowLong(
           IntPtr hWnd,
           int dwStyle);

        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_RIGHT = 0x00001000;
        private const int WS_EX_LEFT = 0x00000000;
        private const int WS_EX_RTLREADING = 0x00002000;
        private const int WS_EX_LTRREADING = 0x00000000;
        private const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        private const int WS_EX_RIGHTSCROLLBAR = 0x00000000;

        private static bool IsRightToLeft(IntPtr handle)
        {
            int style = GetWindowLong(handle, GWL_EXSTYLE);
            return (
               ((style & WS_EX_RIGHT) == WS_EX_RIGHT) ||
               ((style & WS_EX_RTLREADING) == WS_EX_RTLREADING) ||
               ((style & WS_EX_LEFTSCROLLBAR) == WS_EX_LEFTSCROLLBAR));
        }

        private static void FarMargin(IntPtr handle, int margin)
        {
            int message = IsRightToLeft(handle) ? EC_LEFTMARGIN : EC_RIGHTMARGIN;
            if (message == EC_LEFTMARGIN)
            {
                margin = margin & 0xFFFF;
            }
            else
            {
                margin = margin * 0x10000;
            }
            SendMessage(handle, EM_SETMARGINS, message, margin);
        }
    }
}

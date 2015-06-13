using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Key = System.Windows.Input.Key;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            var hWnd = UIAutomationHelper.FindChildWindow("Untitled - Notepad2-mod", "SysListView32");
            hWnd = (IntPtr)0x3080A;
            UIAutomationHelper.PressKeys(hWnd, Key.H, Key.E, Key.L, Key.L, Key.O);
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Win32 wrapper for User32.dll functions
    /// </summary>
    public class Win32
    {
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
                                                     string lpszClass, string lpszWindow);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }

    /// <summary>
    /// This class helps in UI Automation
    /// </summary>
    public class UIAutomationHelper
    {
        /// <summary>
        /// Find a window specified by the window title
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static IntPtr FindWindow(string windowName)
        {
            return Win32.FindWindow(null, windowName);
        }

        /// <summary>
        /// Find a window specified by the class name as well as the window title
        /// </summary>
        /// <param name="className"></param>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static IntPtr FindWindow(string className, string windowName)
        {
            return Win32.FindWindow(className, windowName);
        }

        /// <summary>
        /// Finds a window specified by the window title and set focues on that window
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static IntPtr FindWindowAndFocus(string windowName)
        {
            return UIAutomationHelper.FindWindowAndFocus(null, windowName);
        }

        /// <summary>
        /// Finds a window specified by the class name and window title and set focuses on that window
        /// </summary>
        /// <param name="className"></param>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static IntPtr FindWindowAndFocus(string className, string windowName)
        {
            IntPtr hWindow = Win32.FindWindow(className, windowName);
            Win32.SetForegroundWindow(hWindow);
            return hWindow;
        }

        /// <summary>
        /// Finds a child window
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="childWindowName"></param>
        /// <returns></returns>
        public static IntPtr FindChildWindow(String windowName, String childWindowName)
        {
            return UIAutomationHelper.FindChildWindow(null, windowName, null, childWindowName);
        }

        /// <summary>
        /// Finds a child window
        /// </summary>
        /// <param name="className"></param>
        /// <param name="windowName"></param>
        /// <param name="childClassName"></param>
        /// <param name="childWindowName"></param>
        /// <returns></returns>
        public static IntPtr FindChildWindow(String className, String windowName,
                                    String childClassName, String childWindowName)
        {
            IntPtr hWindow = Win32.FindWindow(className, windowName);
            IntPtr hWindowEx = Win32.FindWindowEx(hWindow,
                               IntPtr.Zero, childClassName, childWindowName);
            return hWindowEx;
        }


        /// <summary>
        /// Simulates pressing a key
        /// </summary>
        /// <param name="hWindow"></param>
        /// <param name="key"></param>
        public static void PressKey(IntPtr hWindow, System.Windows.Input.Key key)
        {
            Win32.PostMessage(hWindow, Win32.WM_KEYDOWN,
                              (IntPtr)System.Windows.Input.KeyInterop.VirtualKeyFromKey(key),
                              IntPtr.Zero);
            //Win32.PostMessage(hWindow, Win32.WM_KEYUP,
            //                  (IntPtr)System.Windows.Input.KeyInterop.VirtualKeyFromKey(key),
            //                  IntPtr.Zero);
        }

        /// <summary>
        /// Simulates pressing several keys
        /// </summary>
        /// <param name="hWindow"></param>
        /// <param name="keys"></param>
        public static void PressKeys(IntPtr hWindow,
                           IEnumerable<System.Windows.Input.Key> keys)
        {
            foreach (var key in keys)
            {
                UIAutomationHelper.PressKey(hWindow, key);
            }
        }

        public static void PressKeys(IntPtr hWnd, params Key[] keys)
        {
            foreach (var key in keys)
                UIAutomationHelper.PressKey(hWnd, key);
        }

    }
}

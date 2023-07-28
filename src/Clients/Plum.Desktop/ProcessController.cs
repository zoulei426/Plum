﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Plum
{
    public static class ProcessController
    {
        private const int SW_SHOW_NORMAL = 1;
        private const int SW_RESTORE = 9;
        private const string PROCESS_NAME = "Plum.Desktop";

        private static Mutex _mutex;

        #region Win32 API functions

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        #endregion Win32 API functions

        public static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual((Visual)sender)).Handle;
            Plum.Properties.Settings.Default.WindowHandle = (long)hwnd;
        }

        public static void Restart()
        {
            Properties.Settings.Default.IsRestarting = true;
            Process.Start($"{Directory.GetCurrentDirectory()}/{PROCESS_NAME}.exe");
            Application.Current.Shutdown();
        }

        public static void CheckSingleton()
        {
            _mutex = new Mutex(true, PROCESS_NAME, out bool isNew);
            if (isNew || Properties.Settings.Default.IsRestarting)
            {
                Properties.Settings.Default.IsRestarting = false;
                return;
            }

            ActivateExistedWindow();
            Application.Current.Shutdown();
        }

        private static void ActivateExistedWindow()
        {
            IntPtr windowHandle = (IntPtr)Properties.Settings.Default.WindowHandle;

            SetForegroundWindow(windowHandle);
            ShowWindowAsync(windowHandle, IsIconic(windowHandle) ? SW_RESTORE : SW_SHOW_NORMAL);
            GetForegroundWindow();
            FlashWindow(windowHandle, true);
        }
    }
}
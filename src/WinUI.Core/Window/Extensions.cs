// ***********************************************************************
// Assembly         : WinUI.Core
// Author           : Patrick Piech
// Created          : 10-02-2021
//
// Last Modified By : Patrick Piech
// Last Modified On : 10-02-2021
// ***********************************************************************
// <copyright file="Extensions.cs" company="WinUI.Core">
//     Copyright (c) BitPrime. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Vanara.PInvoke;

namespace WinUI.Core.Window
{

    /// <summary>
    /// Class Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Moves to.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        /// <exception cref="ArgumentException">window</exception>
        public static void MoveTo(this Microsoft.UI.Xaml.Window window, int X, int Y)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            if (hwnd == IntPtr.Zero)
                throw new ArgumentException(nameof(window));
            User32.MoveWindow(hwnd, X, Y, (int)window.Bounds.Width, (int)window.Bounds.Height, true);
        }

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <exception cref="ArgumentException">window</exception>
        public static void Maximize(this Microsoft.UI.Xaml.Window window)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            if (hwnd == IntPtr.Zero)
                throw new ArgumentException(nameof(window));
            User32.ShowWindowAsync(hwnd, ShowWindowCommand.SW_MAXIMIZE);
        }
    }
}
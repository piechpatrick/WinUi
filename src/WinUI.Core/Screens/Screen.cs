// ***********************************************************************
// Assembly         : WinUI.Core
// Author           : Patrick Piech
// Created          : 10-02-2021
//
// Last Modified By : Patrick Piech
// Last Modified On : 10-02-2021
// ***********************************************************************
// <copyright file="Screen.cs" company="WinUI.Core">
//     Copyright (c) BitPrime. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vanara.PInvoke;

namespace WinUI.Core.Screens
{
    /// <summary>
    /// Class Screen.
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <value>The left.</value>
        public int Left { get; }
        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <value>The right.</value>
        public int Right { get; }
        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <value>The top.</value>
        public int Top { get; }
        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <value>The bottom.</value>
        public int Bottom { get; }
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; }
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        private Screen(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
            Width = right - left;
            Height = bottom - top;
        }
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;Screen&gt;&gt;.</returns>
        public static Task<IEnumerable<Screen>> GetAllAsync()
        {
            var tcs = new TaskCompletionSource<IEnumerable<Screen>>();
            var screens = new HashSet<Screen>();
            try
            {
                User32.EnumDisplayMonitors(HDC.NULL, null, (arg1, arg2, arg3, arg4) =>
                {
                    screens.Add(new Screen(arg3.left, arg3.top, arg3.right, arg3.bottom));
                    return true;
                }, IntPtr.Zero);

                tcs.SetResult(screens);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
    }
}
// ***********************************************************************
// Assembly         : WinUI.Core
// Author           : Patrick Piech
// Created          : 10-02-2021
//
// Last Modified By : Patrick Piech
// Last Modified On : 10-02-2021
// ***********************************************************************
// <copyright file="BusyStateHandler.cs" company="WinUI.Core">
//     Copyright (c) BitPrime. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;

namespace WinUI.Core.State
{
    /// <summary>
    /// Class BusyStateHandler.
    /// </summary>
    public class BusyStateHandler
    {
        /// <summary>
        /// Lock object, used to access <see cref="loadingCounter" />.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// Number of loading start requests
        /// </summary>
        private uint loadingCounter;

        /// <summary>
        /// Gets true, if current mode is Busy
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy => loadingCounter > 0;

        /// <summary>
        /// Raised, when <see cref="IsBusy" /> changes
        /// </summary>
        public event EventHandler BusyStateChanged;

        /// <summary>
        /// Starts busy mode
        /// </summary>
        /// <returns><see cref="IDisposable" />, which will end busy mode on <see cref="IDisposable.Dispose" /></returns>
        public IDisposable StartBusy()
        {
            lock (locker)
            {
                loadingCounter++;
                if (loadingCounter == 1) BusyStateChanged?.Invoke(this, EventArgs.Empty);
            }

            return new DisposableLoadingHandler(this);
        }

        /// <summary>
        /// Ends busy mode
        /// </summary>
        [Obsolete("Use IDisposable returned by StartBusy to wrap code with using()")]
        public void EndBusy()
        {
            EndBusyPrivate();
        }

        /// <summary>
        /// Ends busy mode
        /// </summary>
        private void EndBusyPrivate()
        {
            lock (locker)
            {
                Debug.Assert(loadingCounter > 0);

                loadingCounter--;

                if (loadingCounter == 0) BusyStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The disposable loading handler. It's returned from <see cref="BusyStateHandler.StartBusy" /> and allows to end
        /// loading screen with <see cref="Dispose" />
        /// Implements the <see cref="System.IDisposable" />
        /// </summary>
        /// <seealso cref="System.IDisposable" />
        private class DisposableLoadingHandler : IDisposable
        {
            /// <summary>
            /// <see cref="BusyStateHandler" />, which created and returned this object
            /// </summary>
            private readonly BusyStateHandler parentHandler;

            /// <summary>
            /// True, if <see cref="Dispose" /> was called
            /// </summary>
            private bool isDisposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="DisposableLoadingHandler" /> class.
            /// </summary>
            /// <param name="viewModel">The view model on which we perform Loading</param>
            public DisposableLoadingHandler(BusyStateHandler viewModel)
            {
                parentHandler = viewModel;
            }

            /// <summary>
            /// The dispose. Ends Loading
            /// </summary>
            public void Dispose()
            {
                if (isDisposed)
                    return;

                isDisposed = true;
                parentHandler.EndBusyPrivate();
            }
        }
    }
}
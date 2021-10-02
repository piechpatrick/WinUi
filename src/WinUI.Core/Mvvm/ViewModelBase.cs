// ***********************************************************************
// Assembly         : WinUI.Core
// Author           : Patrick Piech
// Created          : 10-02-2021
//
// Last Modified By : Patrick Piech
// Last Modified On : 10-02-2021
// ***********************************************************************
// <copyright file="ViewModelBase.cs" company="WinUI.Core">
//     Copyright (c) BitPrime. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WinUI.Core.State;

namespace WinUI.Core.Mvvm
{
    /// <summary>
    /// Class ViewModelBase.
    /// </summary>
    public abstract class ViewModelBase : BindableBase
    {
        /// <summary>
        /// The title
        /// </summary>
        private string _title;
        /// <summary>
        /// The busy handler
        /// </summary>
        private readonly BusyStateHandler _busyHandler = new BusyStateHandler();
        /// <summary>
        /// The refresh command
        /// </summary>
        private ICommand _refreshCommand;
        /// <summary>
        /// The save command
        /// </summary>
        private ICommand _saveCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>The refresh command.</value>
        public ICommand RefreshCommand => (this._refreshCommand = new DelegateCommand<object>(
            async (@param) => await this.OnRefresh(@param))) ?? _refreshCommand;
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>The save command.</value>
        public ICommand SaveCommand => (this._saveCommand = new DelegateCommand<object>(
            async (@param) => await this.OnSave(@param))) ?? _saveCommand;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public virtual string Title
        {
            get => this._title;
            set => this.SetProperty(ref this._title, value);
        }
        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy => this._busyHandler.IsBusy;
        /// <summary>
        /// Gets a value indicating whether this <see cref="ViewModelBase"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled => !this._busyHandler.IsBusy;
        /// <summary>
        /// Called when [refresh].
        /// </summary>
        /// <returns>Task.</returns>
        public abstract Task OnRefresh(object @param);
        /// <summary>
        /// Called when [save].
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Task.</returns>
        public abstract Task OnSave(object @param);
        /// <summary>
        /// Loadings the on.
        /// </summary>
        /// <returns>IDisposable.</returns>
        protected IDisposable LoadingOn()
        {
            return this._busyHandler.StartBusy();
        }
    }
}
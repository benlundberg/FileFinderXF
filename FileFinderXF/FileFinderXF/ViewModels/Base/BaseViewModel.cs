﻿using FileFinderXF.Core;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FileFinderXF
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public virtual void OnInitialize()
        {
        }

        public virtual void Appearing()
        {
        }

        public virtual void Disappearing()
        {
        }

        protected string Translate(string key)
        {
            return TranslateHelper.Translate(key);
        }

        protected void ExecuteIfConnected(Action actionToExecute, bool showAlert)
        {
            if (NetStatusHelper.IsConnected)
            {
                actionToExecute();
            }
            else if (showAlert)
            {
                ShowNoNetworkError();
            }
        }

        protected void ShowNoNetworkError(string message = null, string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                title = Translate("GenErr_NoNetworkTitle");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                message = Translate("GenErr_NoNetworkMessage");
            }

            message += " " + Translate("GenErr_CheckYourNetworkMessage");

            ShowAlert(message, title);
        }

        protected void ShowAlert(string message, string title, string okText = "Ok")
        {
            Application.Current.MainPage.DisplayAlert(title, message, okText);
        }

        protected Task<bool> ShowConfirmAsync(string message, string title, string ok = null, string cancel = null)
        {
            ok = ok ?? Translate("Gen_Yes");
            cancel = cancel ?? Translate("Gen_No");

            return Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }

        protected Task<string> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            return Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        private ITranslateHelper translateHelper;
        public ITranslateHelper TranslateHelper => translateHelper ?? (translateHelper = ComponentContainer.Current.Resolve<ITranslateHelper>());

        private INetworkStatusHelper netStatusHelper;
        public INetworkStatusHelper NetStatusHelper => netStatusHelper ?? (netStatusHelper = ComponentContainer.Current.Resolve<INetworkStatusHelper>());

        public INavigation Navigation { get; set; }
        public bool IsNavigating { get; set; }
        public bool IsBusy { get; set; }
        public bool IsNotBusy => !IsBusy;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

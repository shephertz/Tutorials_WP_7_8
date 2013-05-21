/* 
    Copyright (c) 2012 ShepHertz
  
*/

using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TicTacToeAppWarp
{
    /// <summary>
    /// It exposes dependency properties for all settings that can be 
    /// used in data binding by all pages.
    /// </summary>
    public class BasePage : PhoneApplicationPage
    {
        bool _isNewPageInstance = false;

        public BasePage()
        {
            _isNewPageInstance = true;

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // If _isNewPageInstance is true, the page constuctor has been called, so
            // state may need to be restored
            if (_isNewPageInstance)
            {
                (Application.Current as App).ApplicationDataObjectChanged += new EventHandler(basepage_ApplicationDataObjectChanged);
            }

            // Set _isNewPageInstance to false. If the user navigates back to this page
            // and it has remained in memory, this value will continue to be false.
            _isNewPageInstance = false;
        }

        void basepage_ApplicationDataObjectChanged(object sender, EventArgs e)
        {

        }
    }
}

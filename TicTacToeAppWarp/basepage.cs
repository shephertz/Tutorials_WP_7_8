/* 
    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TicTacToeAppWarp
{
    /// <summary>
    /// Base page used by other pages in this application to share implementation of
    /// settings loading. Also exposes dependency properties for all settings that can be 
    /// used in data binding by all pages.
    /// </summary>
    public class basepage : PhoneApplicationPage
    {
        bool _isNewPageInstance = false;

        public basepage()
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

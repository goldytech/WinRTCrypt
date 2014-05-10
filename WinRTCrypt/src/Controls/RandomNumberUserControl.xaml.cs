using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WinrtCrypto.Controls
{
    using Windows.Security.Cryptography;
    using Windows.UI.Popups;

    public sealed partial class RandomNumberUserControl : UserControl
    {
        public RandomNumberUserControl()
        {
            this.InitializeComponent();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var msgDialog = new MessageDialog(CryptographicBuffer.GenerateRandomNumber().ToString());
            var msg = await msgDialog.ShowAsync();
            
        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            uint len;
            if (uint.TryParse(this.TxtLength.Text,out len))
            {
               
                var buffer = CryptographicBuffer.GenerateRandom(len);
                var msgDialog = new MessageDialog(CryptographicBuffer.EncodeToHexString(buffer));
                msgDialog.ShowAsync();

            }
            else
            {
                new MessageDialog("Invalid number").ShowAsync();
                this.TxtLength.Focus(FocusState.Keyboard);
            }
        }
    }
}

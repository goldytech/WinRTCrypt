using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        /// <summary>
        /// The generate random number.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void GenerateRandomNumber(object sender, TappedRoutedEventArgs e)
        {
            // generate random number and show it is message dialog.
            var msgDialog = new MessageDialog(CryptographicBuffer.GenerateRandomNumber().ToString());
            var msg = await msgDialog.ShowAsync();
            
        }

        /// <summary>
        /// Generates the random byte array
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The EventArgs</param>
        void GenerateRandomArray(object sender, TappedRoutedEventArgs e)
        {
            uint len;
            if (uint.TryParse(this.TxtLength.Text, out len))
            {
                // generate the random byte array of specified length.
                var buffer = CryptographicBuffer.GenerateRandom(len);

                // convert the buffer (byte array) into hexadecimal format and show it in message dialog
                var msgDialog = new MessageDialog(CryptographicBuffer.EncodeToHexString(buffer));
                msgDialog.ShowAsync();
            }
            else
            {
                // user inputted invalid number show the message and set back the focus
                new MessageDialog("Invalid number").ShowAsync();
                this.TxtLength.Focus(FocusState.Keyboard);
            }
        }
    }
}

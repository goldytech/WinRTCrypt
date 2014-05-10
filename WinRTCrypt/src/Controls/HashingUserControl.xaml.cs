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
    using Windows.Security.Cryptography.Core;
    using Windows.UI.Popups;

    public sealed partial class HashingUserControl : UserControl
    {
        public HashingUserControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the input string , hash it and then encode it in base64
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Tapped Routed Event Args</param>
        private async void BtnHashConvertTapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtInput.Text))
            {
                return;
            }

            // get the raw input string
            var textToBeHashed = this.TxtInput.Text;
                
            // convert string into binary
            var inputBuffer = CryptographicBuffer.ConvertStringToBinary(textToBeHashed, BinaryStringEncoding.Utf8);

            // declare sha512
            var sha512Hash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512);
                
            // hash the binary data using SHA512 algorithm
            var hashedData = sha512Hash.HashData(inputBuffer);

            // encode the hashdata to Base64 format
            var encodeToBase64String = CryptographicBuffer.EncodeToBase64String(hashedData);
            var msgDialog = new MessageDialog(encodeToBase64String);
                
            // show the output in message dialog
            var msg = await msgDialog.ShowAsync();
        }
    }
}

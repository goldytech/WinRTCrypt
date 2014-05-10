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

    public sealed partial class SymetericEncryptionUserControl : UserControl
    {
        private string symmetricKey;

        private string IV;
        public SymetericEncryptionUserControl()
        {
            this.InitializeComponent();
            this.symmetricKey = GetKey();
            this.IV = GetInitializationVector();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TxtPlain.Text))
            {
                var input = CryptographicBuffer.ConvertStringToBinary(this.TxtPlain.Text, BinaryStringEncoding.Utf8);
                var iv = CryptographicBuffer.DecodeFromBase64String(this.IV);

                var encryptor = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
                var key = encryptor.CreateSymmetricKey(CryptographicBuffer.DecodeFromBase64String(this.symmetricKey));
                var encryptedText = CryptographicEngine.Encrypt(key, input, iv);
                this.TxtCipher.Text = CryptographicBuffer.EncodeToHexString(encryptedText);
            }
        }

        private static string GetKey()
        {
            // generate 1024 bits key
            var buffer = CryptographicBuffer.GenerateRandom(1024);
            return CryptographicBuffer.EncodeToHexString(buffer);
        }

        /// <summary>
        /// The get initialization vector.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetInitializationVector()
        {
            var buffer = CryptographicBuffer.GenerateRandom(16);
            return CryptographicBuffer.EncodeToHexString(buffer);
        }

        /// <summary>
        /// The button tapped 1.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonTapped1(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TxtCipher.Text))
            {
                var input = CryptographicBuffer.DecodeFromHexString(this.TxtCipher.Text);
                var iv = CryptographicBuffer.DecodeFromBase64String(this.IV);

                var decryptor = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
                var key = decryptor.CreateSymmetricKey(CryptographicBuffer.DecodeFromBase64String(this.symmetricKey));
                var decryptedText = CryptographicEngine.Decrypt(key, input, iv);
                this.TxtPlain.Text = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedText);
            }
            else
            {
                new MessageDialog("First encrypt").ShowAsync();
            }
        }
    }
}

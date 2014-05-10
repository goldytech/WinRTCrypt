using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        /// <summary>
        /// The encrypt.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Encrypt(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtPlain.Text))
            {
                return;
            }

            // convert plain text to binary
            var input = CryptographicBuffer.ConvertStringToBinary(this.TxtPlain.Text, BinaryStringEncoding.Utf8);
            var iv = CryptographicBuffer.DecodeFromBase64String(this.IV);

            // select the appropriate algorithm
            var encryptor = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            
            // get the key
            var key = encryptor.CreateSymmetricKey(CryptographicBuffer.DecodeFromBase64String(this.symmetricKey));
            
            // do the encryption
            var encryptedText = CryptographicEngine.Encrypt(key, input, iv);

            // convert the encrypted text into hex format and show it in textbox
            this.TxtCipher.Text = CryptographicBuffer.EncodeToHexString(encryptedText);
        }

        /// <summary>
        /// Generates a random 1024 bit key
        /// </summary>
        /// <returns>A Hex format key</returns>
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
            // a 16 bit random binary value
            var buffer = CryptographicBuffer.GenerateRandom(16);
            return CryptographicBuffer.EncodeToHexString(buffer);
        }

        /// <summary>
        /// The decrypt.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Decrypt(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TxtCipher.Text))
            {
                // as the encrypted string is in hex format decode it and convert back to binary
                var input = CryptographicBuffer.DecodeFromHexString(this.TxtCipher.Text);

                // decode the initialization vector
                var iv = CryptographicBuffer.DecodeFromBase64String(this.IV);

                // specify the algorithm use the same one which was used with encryption
                var decryptor = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
                
                // get the key , same should be used which was used for encryption
                var key = decryptor.CreateSymmetricKey(CryptographicBuffer.DecodeFromBase64String(this.symmetricKey));

                // Do the decryption
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

using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.IO;
using AppSoftware.LicenceEngine.Common;


namespace AppSoftware.LicenceEngine.KeyGenerator
{
    /// <summary>
    /// ActivationKeyXmlWriter allows us to create an xml that can be written to a file and parsed
    /// by the licence engine key generator to decide if the running version is properly licenced.
    /// </summary> 
    internal /*public*/  class ActivationKeyXmlWriter
    {
        /// <summary>
        /// Writes XML for an activation key file to the specified stream.
        /// </summary>
        /// <param name="licenceKeyFileEncryptionKey">An encryption key which the activation key will be encyrpted with</param>
        /// <param name="outputStream">The stream to output activation key XML to.</param>
        /// <param name="licenceKey">
        /// The licence key to put into the activation key file.
        /// </param>
        public void WriteActivationKeyFileXml(string licenceKeyFileEncryptionKey, Stream outputStream, string licenceKey)
        {
            licenceKeyFileEncryptionKey = licenceKeyFileEncryptionKey ?? String.Empty;

            if (String.IsNullOrEmpty(licenceKeyFileEncryptionKey))
            {
                throw new InvalidOperationException("The licence key passed for activation key file generation is empty.");
            }

            string encryptedKey = ActivationKeyEncryption.Encrypt(licenceKey, licenceKeyFileEncryptionKey);

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(outputStream, Encoding.UTF8))
            {
                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("ActivationKey");
                xmlTextWriter.WriteElementString("Key", encryptedKey);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndDocument();
                xmlTextWriter.Flush();
            }
        }
    }
}

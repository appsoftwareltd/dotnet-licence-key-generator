using System;
using System.IO;
using System.Xml;
using AppSoftware.LicenceEngine.Common;

namespace AppSoftware.LicenceEngine.KeyVerification
{
    // GB 2013/05/09 - Activation Key File functionality is not used in installer
    // or program execution as it complicates install, and utilisation of this tool.
    // Class made internal so as not to expose as part of API

    /// <summary>
    /// Provides means of verifying a licence key file
    /// </summary>
    internal /*public*/ class ActivationKeyFileCheck
    {
        /// <summary>
        /// Check activation key file for valid licence key or trial period value
        /// </summary>
        /// <param name="activationKeyFileFullPath">The absolute local path where the activation key file exists</param>
        /// <param name="keyByteSet2"> </param>
        /// <param name="licenceKeyFileEncryptionString">An encryption key which the licence key XML will be decrypted with</param>
        /// <param name="friendlyApplicationName">A friendly application name for error reporting e.g. 'My Licenced Application'</param>
        /// <param name="keyByteSet1"> </param>
        public void CheckActivationKeyFile(string activationKeyFileFullPath, KeyByteSet keyByteSet1, KeyByteSet keyByteSet2, string licenceKeyFileEncryptionString, string friendlyApplicationName)
        {
            if (!File.Exists(activationKeyFileFullPath))
            {
                throw new ActivationKeyFileNotPresentException("The product activation key file '" + activationKeyFileFullPath + "' was expected at '" + activationKeyFileFullPath + "', but was not found. Please visit the vendor website to obtain a product activation key file.");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(activationKeyFileFullPath);

            XmlNodeList activationCodeEncrypted = xmlDoc.GetElementsByTagName("Key");

            var activationKeyEncrypted = activationCodeEncrypted[0].InnerText;

            if (!String.IsNullOrEmpty(activationKeyEncrypted))
            {
                string clearTextActivationKey = String.Empty;

                try
                {
                    clearTextActivationKey = ActivationKeyDecryption.Decrypt(activationKeyEncrypted, licenceKeyFileEncryptionString);
                }
                catch
                {
                    throw new ActivationKeyInvalidException(string.Format("The licence key that allows {0} to run is invalid. Please check that the key file exists in the executing directory, and has not been modified.", friendlyApplicationName));
                }

                // If a check is made, that means that the free trial build period has expired

                if(clearTextActivationKey.ToLower() == "trial")
                {
                    throw new TrialPeriodExpiredException(string.Format("The trial period for {0} has expired.", friendlyApplicationName));
                }

                var pkvLicenceKeyResult = new PkvKeyCheck().CheckKey(clearTextActivationKey, new [] { keyByteSet1, keyByteSet2 }, 8, null);

                if (pkvLicenceKeyResult != PkvLicenceKeyResult.KeyGood)
                {
                    throw new ActivationKeyInvalidException(string.Format("The licence key that allows {0} to run is invalid. Please check that the key file exists in the executing directory, and has not been modified.", friendlyApplicationName));
                }
            }
        }
    }
}

using System;
using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyVerification;

namespace SampleKeyVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // In your application that is to be distributed to the end user (which needs to verify the licence
                // key), only include a subset of the full keyByteSets array used in SampleKeyGenerator. This is so
                // the full set of KeyByteSets is not being compiled into your distributable application code and you have the option
                // of changing the KeyByteSets that are verified.

                var keyByteSets = new[]
                {
                    new KeyByteSet(keyByteNumber: 1, keyByteA: 58, keyByteB: 6, keyByteC: 97),
                    new KeyByteSet(keyByteNumber: 5, keyByteA: 62, keyByteB: 4, keyByteC: 234),
                    new KeyByteSet(keyByteNumber: 8, keyByteA: 6, keyByteB: 88, keyByteC: 32)
                };

                // Enter the key generated in the SampleKeyGenerator console app

                Console.WriteLine("Enter the key generated from the running instance of SampleKeyGenerator:");

                string key = Console.ReadLine();

                var pkvKeyVerifier = new PkvKeyVerifier();

                var pkvKeyVerificationResult = pkvKeyVerifier.VerifyKey(

                    key: key?.Trim(),
                    keyByteSetsToVerify: keyByteSets,

                    // The TOTAL number of KeyByteSets used to generate the licence key in SampleKeyGenerator

                    totalKeyByteSets: 8,

                    // Add blacklisted seeds here if required (these could be user IDs for example)

                    blackListedSeeds: null
                );

                // If the key has been correctly copied, then the key should be reported as valid.

                Console.WriteLine($"Verification result: {pkvKeyVerificationResult}");

                Console.WriteLine("\nPress any key to verify another licence key.");

                Console.ReadKey();
            }
        }
    }
}

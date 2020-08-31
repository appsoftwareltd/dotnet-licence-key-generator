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
                // In your application that is to be distributed to the end user and needs to verify the licence
                // key, only include a subset of the full keyByteSets array used in SampleKeyGenerator. This is so
                // the full key is not being compiled into your distributable application code and you have the option
                // of changing the KeyByteSets that are verified.

                var keyByteSets = new[]
                {
                    new KeyByteSet(keyByteNo: 1, keyByteA: 58, keyByteB: 6, keyByteC: 97),
                    new KeyByteSet(keyByteNo: 5, keyByteA: 62, keyByteB: 4, keyByteC: 234),
                    new KeyByteSet(keyByteNo: 8, keyByteA: 6, keyByteB: 88, keyByteC: 32)
                };

                Console.WriteLine("Enter the key generated from the running instance of SampleKeyGenerator:");

                string key = Console.ReadLine();

                var pkvKeyVerifier = new PkvKeyVerifier();

                var pkvKeyVerificationResult = pkvKeyVerifier.VerifyKey(
                    key: key?.Trim(),
                    keyByteSetsToVerify: keyByteSets,

                    // The number of KeyByteSets used to generate the licence key in SampleKeyGenerator

                    totalKeyByteSets: 8,

                    // Add blacklisted seeds here if required (these could be user IDs for example)

                    blackListedSeeds: null
                );

                Console.WriteLine($"Verification result: {pkvKeyVerificationResult}");

                Console.WriteLine("\nPress any key to verify another licence key.");

                Console.ReadKey();
            }
        }
    }
}

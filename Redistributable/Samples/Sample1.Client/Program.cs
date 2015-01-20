using System;
using System.Collections.Generic;
using System.Text;
using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyVerification;
using Sample1.Server;

namespace Sample1.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // In this sample, we will use the user id as our seed. This could be any integer that you can retrive in the future,
            // such as application id or even a combination of user id and application id

            Console.WriteLine("Please enter a dummy user id (integer). This will be used as our seed:");

            int userId = int.Parse(Console.ReadLine());

            // We will use the LicenceServer project in this sample to generate a key.
            // In production code, the licence server mechanism would be a separate application
            // from the application that verifies the key. This is important so as to protect the
            // full KeyByteSet array that was used to generate the key.

            var licenceServer = new LicenceServer();

            // Generate the licence key using the seed

            string licenceKeyStr = licenceServer.GenerateLicenceKey(userId);

            Console.WriteLine("\nLicence key generated: " + licenceKeyStr);
            Console.WriteLine("\nNow we will verify the licence key. Please type the licence key printed above: ");

            var result = PkvLicenceKeyResult.KeyInvalid;

            while (result != PkvLicenceKeyResult.KeyGood)
            {
                string userEnteredLicenceKeyStr = Console.ReadLine();

                var pkvKeyCheck = new PkvKeyCheck();

                // Here we recreate a subset of the full original KeyByteSet array that
                // was used to create the licence key. Note the argument to keyByteNo in 
                // each matches that in the full KeyByteSet array.

                var keyBytes = new[] {

                    new KeyByteSet(5, 165, 15, 132), 
                    new KeyByteSet(6, 128, 175, 213)
                };

                result = pkvKeyCheck.CheckKey(userEnteredLicenceKeyStr, keyBytes, 8, null);

                if (result != PkvLicenceKeyResult.KeyGood)
                {
                    Console.WriteLine("\nResult is: " + result.ToString() + ". Please try again.");
                }
            }

            Console.WriteLine("\nResult is: " + result.ToString() + ". Press any key to exit.");
            Console.ReadKey();
        }
    }
}

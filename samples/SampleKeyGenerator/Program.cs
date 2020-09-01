using System;
using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyGenerator;

namespace SampleKeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // Here in SampleKeyGenerator is the full set of KeyByteSet used to generate the licence key.
                // You should change these values in your solution.

                var keyByteSets = new[]
                {
                    new KeyByteSet(keyByteNumber: 1, keyByteA: 58, keyByteB: 6, keyByteC: 97),
                    new KeyByteSet(keyByteNumber: 2, keyByteA: 96, keyByteB: 254, keyByteC: 23),
                    new KeyByteSet(keyByteNumber: 3, keyByteA: 11, keyByteB: 185, keyByteC: 69),
                    new KeyByteSet(keyByteNumber: 4, keyByteA: 2, keyByteB: 93, keyByteC: 41),
                    new KeyByteSet(keyByteNumber: 5, keyByteA: 62, keyByteB: 4, keyByteC: 234),
                    new KeyByteSet(keyByteNumber: 6, keyByteA: 200, keyByteB: 56, keyByteC: 49),
                    new KeyByteSet(keyByteNumber: 7, keyByteA: 89, keyByteB: 45, keyByteC: 142),
                    new KeyByteSet(keyByteNumber: 8, keyByteA: 6, keyByteB: 88, keyByteC: 32)
                };

                // A unique key will be created for the seed value. This value could be a user ID or something
                // else depending on your application logic.

                int seed = new Random().Next(0, int.MaxValue);

                Console.WriteLine("Seed (for example user ID) is:");
                Console.WriteLine(seed);

                // Generate the key ... 

                var pkvLicenceKeyGenerator = new PkvKeyGenerator();

                string licenceKey = pkvLicenceKeyGenerator.MakeKey(seed, keyByteSets);

                Console.WriteLine("Generated licence key is:");
                Console.WriteLine(licenceKey);

                // The values output can now be copied into the SampleKeyVerification console app to demonstrate
                // verification.

                Console.WriteLine("\nCopy these values to a running instance of SampleKeyVerification to test key verification.");

                Console.WriteLine("\nPress any key to generate another licence key.");

                Console.ReadKey();
            }
        }
    }
}

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

                var keyByteSets = new[]
                {
                    new KeyByteSet(keyByteNo: 1, keyByteA: 58, keyByteB: 6, keyByteC: 97),
                    new KeyByteSet(keyByteNo: 2, keyByteA: 96, keyByteB: 254, keyByteC: 23),
                    new KeyByteSet(keyByteNo: 3, keyByteA: 11, keyByteB: 185, keyByteC: 69),
                    new KeyByteSet(keyByteNo: 4, keyByteA: 2, keyByteB: 93, keyByteC: 41),
                    new KeyByteSet(keyByteNo: 5, keyByteA: 62, keyByteB: 4, keyByteC: 234),
                    new KeyByteSet(keyByteNo: 6, keyByteA: 200, keyByteB: 56, keyByteC: 49),
                    new KeyByteSet(keyByteNo: 7, keyByteA: 89, keyByteB: 45, keyByteC: 142),
                    new KeyByteSet(keyByteNo: 8, keyByteA: 6, keyByteB: 88, keyByteC: 32)
                };

                // A unique key will be created for the seed value. This value could be a user ID or something
                // else depending on your application logic.

                int seed = new Random().Next(0, int.MaxValue);

                Console.WriteLine("Seed (for example user ID) is:");
                Console.WriteLine(seed);

                // Generate the key

                var pkvLicenceKeyGenerator = new PkvKeyGenerator();

                string licenceKey = pkvLicenceKeyGenerator.MakeKey(seed, keyByteSets);

                Console.WriteLine("Generated licence key is:");
                Console.WriteLine(licenceKey);

                Console.WriteLine("\nCopy these values to a running instance of SampleKeyVerification to test key verification.");

                Console.WriteLine("\nPress any key to generate another licence key.");

                Console.ReadKey();
            }
        }
    }
}

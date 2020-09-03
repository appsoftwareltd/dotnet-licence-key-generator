# .NET Licence Key Generator

## Licence Key Generation and Verification for .NET

A common requirement for software vendors is the inclusion of a system for generating and verifying passwords, which can be bundled with distributed desktop software, or for granting access to online services. App Software's .NET Licence Key Generator provides a robust, lightweight and tested means of solving this problem.

.NET Licence Key Generator implements a 'Partial Number Verification System', which along with other features, provides a defence against key generators which may be built to attempt to crack your licence key system.

For support and consulting related to this project, contact mail@appsoftware.com

### Key benefits of implementing your licence system with the .NET Licence Key Generator are: ###

- No 'Phone Home' to verify licence keys is required.
- Ability to revoke licence keys if keys found to have been distributed without authorisation.
- Compiled key verification code cannot be fully reproduced to allow the creation of a key generator (keygen) as full key data is not required for verification.
- Light weight and fast code execution, tested up to 1,000,000 key generation and verification cycles in 10.2 seconds.

## Compatibility

.NET Licence Key Generator targets .NET Standard 1.0. This means that this library can be used with:

- .NET Core 1.0 +
- .NET Framework 4.5 +

For full .NET Standard implementation support see: https://docs.microsoft.com/en-us/dotnet/standard/net-standard

Note: The legacy .NET Framework 2.0 version is retained in this repository under `/legacy-netfx` but be aware that the API was changed in the port to .NET standard, will vary significantly from the samples included below and will not be maintained going forward.

## Implementation

In your application that is responsible for generating the licence keys, reference the AppSoftware.LicenceEngine.KeyGenerator NuGet package.

https://www.nuget.org/packages/AppSoftware.LicenceEngine.KeyGenerator

    dotnet add package AppSoftware.LicenceEngine.KeyGenerator --version 1.3.0

In the application for which a licence key is to be verified (this could be an application you distribute to your end user), reference the AppSoftware.LicenceEngine.KeyVerification NuGet package. Do not reference the AppSoftware.LicenceEngine.KeyGenerator in code that will will be compiled into software that will be distributed to the end user.

https://www.nuget.org/packages/AppSoftware.LicenceEngine.KeyVerification

    dotnet add package AppSoftware.LicenceEngine.KeyVerification --version 1.3.0

## Samples

Sample code is included in this repository under ```/samples```    

### Licence Key Generator Application Sample

A full sample application for generating licence keys is as follows:

```csharp
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

                var pkvKeyGenerator = new PkvKeyGenerator();

                string licenceKey = pkvKeyGenerator.MakeKey(seed, keyByteSets);

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
```

### Licence Key Verification Application Sample

Verifying the licence key generated in the above sample code can be achieved as below:

```csharp
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
```

## How it works

To generate a licence key we essentially need two things:

- A set of arbitrary 'key' bytes that are unique to the application that we want to protect. You are responsible for choosing a random set of bytes to use as your 'keys'
- A seed that is unique to the context in which a key is being generated, e.g. 1 licence key for 1 user, or 1 licence key for 1 user application id combination.

Together, the seed and the byte keys will control the key that is produced. What is unique to this system, is that when verifying the licence key, you only test a subset of the total byte keys used to generate the full key. This means that the full set of key bytes used to create the licence key does not have to be included in the distributed software, and therefore it is harder to create a keygen to crack the keys you generate. An attacker does not have all the information to fully reverse engineer your key verification system.

With this system, you are able to vary the keys tested on new builds for published versions of your software, and you have the option of generating new byte key sets for new versions.

You can vary the number of byte keys used to make longer, stronger keys. Your validation code will need to know how many keys were used in the generation of a key.

Remember that no distributed software can be protected 100% against attackers. Whatever the technique used to generate a licence key system, all distributed software has the potential to be decompiled and modified to that licence key verification is skipped entirely. Depending on the context in which you distribute your software, you may need to employ obfuscation and other security techniques to make this less likely. Our system provides a simple programming interface, that helps to create a user friendly, attacker resistant means of implementing licence key functionality in your application.

## Terms of use

App Software Ltd and our partners accept no liability for any loss or damage to physical property or intellectual property arising as a result of using our products.

Users of our software, licensed or otherwise are expected to undertake their own thorough testing and assessment for suitability of our products before integrating with their solutions.

### Funding and Sponsorship:

This project is currently free open source distributed under the below licence.

**If you are using this project within or on behalf of a for-profit commercial organisation, or including in software that is to be redistributed we ask you to sponsor this organisation so that we can continue to maintain and develop this and other open source projects further.** 

Sponsorship can be set up via GitHub sponsors:

https://github.com/sponsors/appsoftwareltd

Thank you, your support is greatly appreciated!

### Licence:

Copyright © 2020 http://www.appsoftware.com
Released under the Creative Commons Attribution 2.0 UK: England & Wales license:
[https://creativecommons.org/licenses/by/2.0/uk/](https://creativecommons.org/licenses/by/2.0/uk/)

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



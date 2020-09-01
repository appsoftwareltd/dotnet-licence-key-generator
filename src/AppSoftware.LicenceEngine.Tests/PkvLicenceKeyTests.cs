using System;
using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyGenerator;
using AppSoftware.LicenceEngine.KeyVerification;
using NUnit.Framework;

namespace AppSoftware.LicenceEngine.Tests
{
    [TestFixture]
    public class PkvLicenceKeyTests
    {
        [Test]
        public void TestPkvLicenceKeyGenerationAndVerification()
        {
            var pkvLicenceKeyGenerator = new PkvKeyGenerator();

            var pkvKeyVerifier = new PkvKeyVerifier();

            string key;

            KeyByteSet[] keyByteSets = {
                                           new KeyByteSet(keyByteNumber: 1, keyByteA: 58,  keyByteB: 6,   keyByteC: 97),
                                           new KeyByteSet(keyByteNumber: 2, keyByteA: 96,  keyByteB: 254, keyByteC: 23),
                                           new KeyByteSet(keyByteNumber: 3, keyByteA: 11,  keyByteB: 185, keyByteC: 69),
                                           new KeyByteSet(keyByteNumber: 4, keyByteA: 2,   keyByteB: 93,  keyByteC: 41),
                                           new KeyByteSet(keyByteNumber: 5, keyByteA: 62,  keyByteB: 4,   keyByteC: 234),
                                           new KeyByteSet(keyByteNumber: 6, keyByteA: 200, keyByteB: 56,  keyByteC: 49),
                                           new KeyByteSet(keyByteNumber: 7, keyByteA: 89,  keyByteB: 45,  keyByteC: 142),
                                           new KeyByteSet(keyByteNumber: 8, keyByteA: 6,   keyByteB: 88,  keyByteC: 32)
                                       };

            // Change these to a random key byte set from the above array to test key verification with

            KeyByteSet kbs1 = keyByteSets[3];
            KeyByteSet kbs2 = keyByteSets[7];
            KeyByteSet kbs3 = keyByteSets[4];

            // The check project also uses a class called KeyByteSet, but with
            // separate name spacing to achieve single self contained dll

            KeyByteSet keyByteSet1 = new KeyByteSet(kbs1.KeyByteNumber, kbs1.KeyByteA, kbs1.KeyByteB, kbs1.KeyByteC); // Change no to test others
            KeyByteSet keyByteSet2 = new KeyByteSet(kbs2.KeyByteNumber, kbs2.KeyByteA, kbs2.KeyByteB, kbs2.KeyByteC);
            KeyByteSet keyByteSet3 = new KeyByteSet(kbs3.KeyByteNumber, kbs3.KeyByteA, kbs3.KeyByteB, kbs3.KeyByteC);

            var random = new Random();

            for (int i = 0; i < 10000; i++)
            {
                int seed = random.Next(0, int.MaxValue);

                key = pkvLicenceKeyGenerator.MakeKey(seed, keyByteSets);

                // Check that check sum validation passes

                Assert.True(pkvKeyVerifier.CheckKeyChecksum(key, keyByteSets.Length));

                // Check using full check method

                Assert.True(pkvKeyVerifier.VerifyKey(
                                            key,
                                            new[] { keyByteSet1, keyByteSet2, keyByteSet3 },
                                            keyByteSets.Length,
                                            null
                                        ) == PkvKeyVerificationResult.KeyIsValid, "Failed on iteration " + i
                            );

                // Check that erroneous check sum validation fails

                Assert.False(pkvKeyVerifier.CheckKeyChecksum(key.Remove(23, 1) + "A", keyByteSets.Length)); // Change key by replacing 17th char
            }

            // Check a few random inputs

            Assert.False(pkvKeyVerifier.VerifyKey("adcsadrewf",
                                            new[] { keyByteSet1, keyByteSet2 },
                                            keyByteSets.Length,
                                            null) == PkvKeyVerificationResult.KeyIsValid
                        );
            Assert.False(pkvKeyVerifier.VerifyKey("",
                                            new[] { keyByteSet1, keyByteSet2 },
                                            keyByteSets.Length,
                                            null) == PkvKeyVerificationResult.KeyIsValid
                        );
            Assert.False(pkvKeyVerifier.VerifyKey("123",
                                            new[] { keyByteSet1, keyByteSet2 },
                                            keyByteSets.Length,
                                            null) == PkvKeyVerificationResult.KeyIsValid
                        );
            Assert.False(pkvKeyVerifier.VerifyKey("*()",
                                            new[] { keyByteSet1, keyByteSet2 },
                                            keyByteSets.Length,
                                            null) == PkvKeyVerificationResult.KeyIsValid
                        );
            Assert.False(pkvKeyVerifier.VerifyKey("dasdasdasgdjwqidqiwd21887127eqwdaishxckjsabcxjkabskdcbq2e81y12e8712",
                                            new[] { keyByteSet1, keyByteSet2 },
                                            keyByteSets.Length,
                                            null) == PkvKeyVerificationResult.KeyIsValid
                        );
        }

        [Test]
        public void TestPkvLicenceKeyGenerationAndVerificationWithRandomKeyBytesKeyByteQtyAndVerificationKeyByteSelection()
        {
            var pkvLicenceKeyGenerator = new PkvKeyGenerator();

            var pkvKeyVerifier = new PkvKeyVerifier();

            var random = new Random();

            for (int i = 0; i < 10000; i++)
            {
                int randomKeyByteSetsLength = random.Next(2, 400);

                KeyByteSet[] keyByteSets = new KeyByteSet[randomKeyByteSetsLength];

                for (int j = 0; j < randomKeyByteSetsLength; j++)
                {
                    var kbs = new KeyByteSet
                                  (
                                      j + 1,
                                      (byte) random.Next(0, 256),
                                      (byte) random.Next(0, 256),
                                      (byte) random.Next(0, 256)
                                  );

                    keyByteSets[j] = kbs;
                }

                // Select a random key byte set to test key verification with

                KeyByteSet kbs1 = keyByteSets[random.Next(0, randomKeyByteSetsLength)];
                KeyByteSet kbs2 = keyByteSets[random.Next(0, randomKeyByteSetsLength)];

                // The check project also uses a class called KeyByteSet, but with
                // separate name spacing to achieve single self contained dll

                KeyByteSet keyByteSet1 = new KeyByteSet(kbs1.KeyByteNumber, kbs1.KeyByteA, kbs1.KeyByteB, kbs1.KeyByteC); // Change no to test others
                KeyByteSet keyByteSet2 = new KeyByteSet(kbs2.KeyByteNumber, kbs2.KeyByteA, kbs2.KeyByteB, kbs2.KeyByteC);

                int seed = random.Next(0, int.MaxValue);

                string key = pkvLicenceKeyGenerator.MakeKey(seed, keyByteSets);

                // Check that check sum validation passes

                Assert.True(pkvKeyVerifier.CheckKeyChecksum(key, keyByteSets.Length));

                // Check using full check method

                Assert.True(pkvKeyVerifier.VerifyKey(
                                            key,
                                            new[] { keyByteSet1, keyByteSet2 },
                                            keyByteSets.Length,
                                            null
                                        ) == PkvKeyVerificationResult.KeyIsValid, "Failed on iteration " + i
                            );

            }
        }
    }
}

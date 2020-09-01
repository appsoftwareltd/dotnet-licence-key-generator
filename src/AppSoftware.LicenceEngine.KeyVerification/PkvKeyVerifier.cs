using System;
using AppSoftware.LicenceEngine.Common;

namespace AppSoftware.LicenceEngine.KeyVerification
{
    /// <summary>
    /// Provides methods for verifying a licence key.
    /// </summary>
    public class PkvKeyVerifier : PkvKeyBase
    {
        /// <summary>
        /// Verify a given key for validity
        /// </summary>
        /// <param name="key">The full key</param>
        /// <param name="keyByteSetsToVerify">The KeyByteSets that are to be tested in this verification</param>
        /// <param name="totalKeyByteSets">The total number of KeyBytes used to make the key</param>
        /// <param name="blackListedSeeds">Any seed values (hex string representation) that should be banned</param>
        /// <returns></returns>
        public PkvKeyVerificationResult VerifyKey(
            string key, 
            KeyByteSet[] keyByteSetsToVerify,
            int totalKeyByteSets, 
            string[] blackListedSeeds
        )
        {
            key = FormatKeyForCompare(key);

            PkvKeyVerificationResult pkvKeyVerificationResult = PkvKeyVerificationResult.KeyIsInvalid;

            bool checksumPass = CheckKeyChecksum(key, totalKeyByteSets);

            if (checksumPass)
            {
                if (blackListedSeeds != null && blackListedSeeds.Length > 0)
                {
                    // Test key against our black list

                    // Example black listed seed: 111111 (Hex val). Producing keys with the same 
                    // seed and key bytes will produce the same key, so using a seed such as a user id
                    // can provide a mechanism for tracking the source of any keys that are found to
                    // be used out of licence terms.

                    for (int i = 0; i < blackListedSeeds.Length; i++)
                    {
                        if (key.StartsWith(blackListedSeeds[i]))
                        {
                            pkvKeyVerificationResult = PkvKeyVerificationResult.KeyBlackListed;
                        }
                    }
                }

                if (pkvKeyVerificationResult != PkvKeyVerificationResult.KeyBlackListed)
                {
                    // At this point, the key is either valid or forged,
                    // because a forged key can have a valid checksum.
                    // We now test the "bytes" of the key to determine if it is
                    // actually valid.

                    // When building your release application, select a subset of the KeyByteSets to verify.
                    // By not compiling in each KeyByteSet, it is harder for an attacker to build a keygen that
                    // will produce valid keys.  If an invalid keygen is released, you
                    // simply change which byte checks are compiled in, and any serial
                    // number built with the fake keygen no longer works.

                    pkvKeyVerificationResult = PkvKeyVerificationResult.KeyPhoney;

                    int seed;

                    bool seedParsed = int.TryParse(key.Substring(0, 8), System.Globalization.NumberStyles.HexNumber, null, out seed);

                    if (seedParsed)
                    {
                        foreach (var keyByteSet in keyByteSetsToVerify)
                        {
                            var keySubstringStart = GetKeySubstringStart(keyByteSet.KeyByteNumber);

                            if (keySubstringStart - 1 > key.Length)
                            {
                                throw new InvalidOperationException("The KeyByte check position is out of range. You may have specified a check KeyByteNumber that did not exist in the original key generation.");
                            }
                            
                            var keyBytes = key.Substring(keySubstringStart, 2);

                            var b = GetKeyByte(seed, keyByteSet.KeyByteA, keyByteSet.KeyByteB, keyByteSet.KeyByteC);

                            if (keyBytes != b.ToString("X2"))
                            {
                                // If true, then it means the key is either good, or was made
                                // with a keygen derived from "this" release.

                                return pkvKeyVerificationResult; // Return result in failed state 
                            }
                        }

                        pkvKeyVerificationResult = PkvKeyVerificationResult.KeyIsValid;
                    }
                }
            }

            return pkvKeyVerificationResult;
        }

        /// <summary>
        /// Short hand way of creating pattern 8, 10, 12, 14
        /// </summary>
        /// <param name="keyByteNumber"></param>
        /// <returns></returns>
        private int GetKeySubstringStart(int keyByteNumber)
        {
            return (keyByteNumber * 2) + 6;
        }

        /// <summary>
        /// Indicate if the check sum portion of the key is valid
        /// </summary>
        /// <param name="key"></param>
        /// <param name="totalKeyByteSets"> </param>
        /// <returns></returns>
        public bool CheckKeyChecksum(string key, int totalKeyByteSets)
        {
            bool result = false;

            string formattedKey = FormatKeyForCompare(key);

            if (formattedKey.Length == (8 + 4 + (2 * totalKeyByteSets))) // First 8 are seed, 4 for check sum, plus 2 for each KeyByte
            {
                int keyLessChecksumLength = formattedKey.Length - 4;

                string checkSum = formattedKey.Substring(keyLessChecksumLength, 4); // Last 4 chars are checksum

                string keyWithoutChecksum = formattedKey.Substring(0, keyLessChecksumLength);

                result = GetChecksum(keyWithoutChecksum) == checkSum;
            }

            return result;
        }
    }
}

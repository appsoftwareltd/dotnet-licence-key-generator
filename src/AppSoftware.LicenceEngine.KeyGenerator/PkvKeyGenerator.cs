
// This Key Generator was built with the assistance of the article linked below (credit to Brandon Staggs). 
// Brandon Staggs article provided example in Delphi. Some functionality has been ported to C#
// to create this key generator.

// http://www.brandonstaggs.com/2007/07/26/implementing-a-partial-serial-number-verification-system-in-delphi/

// Delphi -> .NET mapping concept notes

// Delphi shift right http://www.delphibasics.co.uk/RTL.asp?Name=Shr
// C# Shift right http://www.blackwasp.co.uk/CSharpShiftOperators.aspx
// Delphi Word - equivalent to ushort http://www.delphibasics.co.uk/RTL.asp?Name=Word
// Delphi Decrement - https://www.google.co.uk/search?q=delphi+Dec&oq=delphi+Dec&sugexp=chrome,mod=3&sourceid=chrome&ie=UTF-8
// C# string to hex http://stackoverflow.com/questions/5426582/turn-byte-into-two-digit-hexadecimal-number-just-using-tostring

using System;
using System.Collections.Generic;
using AppSoftware.LicenceEngine.Common;

namespace AppSoftware.LicenceEngine.KeyGenerator
{
    // Note - The AppSoftware.LicenceEngine.KeyGenerator package should not be referenced in the protected
    // application. This is to make it as difficult as possible for an attacker to discover the mechanism required
    // to create a keygen.

    public class PkvKeyGenerator : PkvKeyBase
    {
        /// <summary>
        /// Generate a new key given a seed value. This seed should be unique so that where licences are blacklisted, 
        /// we only blacklist one key. Store the seed when generating new licences, or put in place some other mechanism so that
        /// the key will not be repeated for the same application. This seed does not necessarily have to be randomised.
        /// </summary>
        /// <param name="seed">Random number</param>
        /// <param name="keyByteSets">A list of key bytes that will be used to produce the key</param>
        /// <returns></returns>
        public string MakeKey(int seed, KeyByteSet[] keyByteSets)
        {
            if (keyByteSets.Length < 2)
            {
                throw new InvalidOperationException("The KeyByteSet array must be of length 2 or greater.");
            }

            // Check that array is in correct order as this will cause errors if passed in incorrectly

            Array.Sort(keyByteSets, new KeyByteSetComparer());

            bool allKeyByteNumbersDistinct = true;

            var keyByteCheckedNos = new List<int>();

            int maxKeyByteNumber = 0;

            foreach (var keyByteSet in keyByteSets)
            {
                if (!(keyByteCheckedNos.Contains(keyByteSet.KeyByteNumber)))
                {
                    keyByteCheckedNos.Add(keyByteSet.KeyByteNumber);

                    if (keyByteSet.KeyByteNumber > maxKeyByteNumber)
                    {
                        maxKeyByteNumber = keyByteSet.KeyByteNumber;
                    }
                }
                else
                {
                    allKeyByteNumbersDistinct = false;
                    break;
                }
            }

            if (!allKeyByteNumbersDistinct)
            {
                throw new InvalidOperationException("The KeyByteSet array contained at least 1 item with a duplicate KeyByteNumber value.");
            }

            if (maxKeyByteNumber != keyByteSets.Length)
            {
                throw new InvalidOperationException("The values for KeyByteNumber in each KeyByteSet item must be sequential and start with the number 1.");
            }

            // Note these seed value, with random numbers need to be repeated in check function.
            // The more of these values the better, but they will also increase the length of the
            // key by 2 chars each. Changing the length of the key is not something to be done
            // without testing, since some operations depend on certain portions of the key
            // being found at specific indexes.

            var keyBytes = new byte[keyByteSets.Length];

            for (int i = 0; i < keyByteSets.Length; i++)
            {
                keyBytes[i] = GetKeyByte(
                                    seed,
                                    keyByteSets[i].KeyByteA,
                                    keyByteSets[i].KeyByteB,
                                    keyByteSets[i].KeyByteC
                              );
            }

            // The key string begins with a hexidecimal string of the seed

            string result = seed.ToString("X8"); // 8 digit hex;

            for (int i = 0; i < keyBytes.Length; i++)
            {
                result = result + keyBytes[i].ToString("X2");
            }

            result = result + GetChecksum(result);

            // Insert hyphens every 6 chars for readability

            int startPos = 7;

            while (startPos < (result.Length - 1))
            {
                result = result.Insert(startPos, "-");

                startPos = startPos + 7;
            }

            return result;
        }
    }
}
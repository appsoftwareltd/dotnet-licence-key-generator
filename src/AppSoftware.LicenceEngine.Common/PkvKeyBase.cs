using System;

namespace AppSoftware.LicenceEngine.Common
{
    public class PkvKeyBase
    {
        ////////////////////////////////////////////////////
        // Code below from here is duplicated across both
        // private and public projects / dlls
        ////////////////////////////////////////////////////

        /// <summary>
        /// Strip padding chars for comparison
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static string FormatKeyForCompare(string key)
        {
            if (key == null)
            {
                key = string.Empty;
            }

            // Replace -, space etc, upper case

            return key.Trim().ToUpper().Replace("-", String.Empty).Replace(" ", String.Empty);
        }

        /// <summary>
        /// Given a seed and some input bytes, generate a single byte to return. This should 
        /// be used with randomised data, that can be represented to retrieve the same key.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        protected byte GetKeyByte(long seed, byte a, byte b, byte c)
        {
            int aTemp = a % 25;
            int bTemp = b % 3;

            long result;

            if ((a % 2) == 0)
            {
                result = ((seed >> aTemp) & 0xFF) ^ ((seed >> bTemp) | c);
            }
            else
            {
                result = ((seed >> aTemp) & 0xFF) ^ ((seed >> bTemp) & c);
            }

            return (byte)result;
        }

        /// <summary>
        /// Generate a new checksum for a key
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected string GetChecksum(string str)
        {
            ushort left = 0x56;
            ushort right = 0xAF;

            if (str.Length > 0)
            {
                // 0xFF hex for 255

                for (int cnt = 0; cnt < str.Length; cnt++)
                {
                    right = (ushort)(right + Convert.ToByte(str[cnt]));

                    if (right > 0xFF)
                    {
                        right -= 0xFF;
                    }

                    left += right;

                    if (left > 0xFF)
                    {
                        left -= 0xFF;
                    }
                }
            }

            ushort sum = (ushort)((left << 8) + right);

            return sum.ToString("X4"); // 4 char hex
        }
    }
}

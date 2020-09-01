using System;

namespace AppSoftware.LicenceEngine.Common
{
    /// <summary>
    /// KeyByteSet data container.
    /// </summary>
    public class KeyByteSet
    {
        /// <summary>
        /// Constructor for KeyByteSet
        /// </summary>
        /// <param name="keyByteNumber">An integer. Minimum allowed value is 1.</param>
        /// <param name="keyByteA">A randomly selected byte value</param>
        /// <param name="keyByteB">A randomly selected byte value</param>
        /// <param name="keyByteC">A randomly selected byte value</param>
        public KeyByteSet(int keyByteNumber, byte keyByteA, byte keyByteB, byte keyByteC)
        {
            if (keyByteNumber < 1)
            {
                throw new ArgumentException($"{nameof(keyByteNumber)} should be greater or equal to 1", nameof(keyByteNumber));
            }

            KeyByteNumber = keyByteNumber;
            KeyByteA = keyByteA;
            KeyByteB = keyByteB;
            KeyByteC = keyByteC;
        }

        public int KeyByteNumber { get; }
        public byte KeyByteA { get; }
        public byte KeyByteB { get; }
        public byte KeyByteC { get; }
    }
}
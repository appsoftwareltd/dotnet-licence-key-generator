using System;
using System.Collections;
using AppSoftware.LicenceEngine.Common;

namespace AppSoftware.LicenceEngine.KeyGenerator
{
    public class KeyByteSetComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var keyByteSetX = (KeyByteSet) x;
            var keyByteSetY = (KeyByteSet) y;

            if (keyByteSetX == null)
            {
                throw new ArgumentNullException(nameof(keyByteSetX));
            }

            if (keyByteSetY == null)
            {
                throw new ArgumentNullException(nameof(keyByteSetY));
            }

            if (keyByteSetX.KeyByteNumber > keyByteSetY.KeyByteNumber)
            {
                return 1;
            }

            if (keyByteSetX.KeyByteNumber < keyByteSetY.KeyByteNumber)
            {
                return -1;
            }

            return 0;
        }
    }

}
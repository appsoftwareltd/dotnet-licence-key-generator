using System;
using System.Collections.Generic;
using System.Text;
using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyGenerator;

namespace Sample1.Server
{
    public class LicenceServer
    {
        public string GenerateLicenceKey(int userId)
        {
            var keyGenerator = new PkvLicenceKeyGenerator();

            // The KeyByteSet objects are the secret byte groups that are built in
            // to the licence key. On verification of the licence
            // key, we will test against a subset of these KeyByteSet objects.

            // Important: The full set of KeyByteSet objects should not be available
            // in any publicly distributed code or binaries. Nor should
            // the AppSoftware.LicenceEngine.KeyGenerator.dll binary file.

            var keyBytes = new [] {
                                     
                                     new KeyByteSet(1, 254, 122, 96), 
                                     new KeyByteSet(2, 54, 124, 222),
                                     new KeyByteSet(3, 119, 142, 132),
                                     new KeyByteSet(4, 128, 122, 10),
                                     new KeyByteSet(5, 165, 15, 132),
                                     new KeyByteSet(6, 128, 175, 213),
                                     new KeyByteSet(7, 7, 244, 132 ),
                                     new KeyByteSet(8, 128, 122, 251)
                                  };

            string key = keyGenerator.MakeKey(userId, keyBytes);

            // The key can now be presented to the user.

            return key;
        }
    }
}

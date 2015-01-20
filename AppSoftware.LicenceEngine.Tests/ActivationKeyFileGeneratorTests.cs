using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyGenerator;
using NUnit.Framework;
using System.IO;

namespace AppSoftware.LicenceEngine.Tests
{
    [TestFixture]
    public class ActivationKeyFileGeneratorTests
    {
        [Test]
        public void Test_create_licence_engine_activation_xml_contents()
        {
            var activationData = new ActivationKeyXmlWriter();

            // These are the key bytes for this application

            KeyByteSet[] keyByteSets = new[]
                                               {
                                                   new KeyByteSet(1, 58, 6, 97),
                                                   new KeyByteSet(2, 96, 254, 23),
                                                   new KeyByteSet(3, 11, 185, 69), 
                                                   new KeyByteSet(4, 2, 93, 41),
                                                   new KeyByteSet(5, 62, 4, 234),
                                                   new KeyByteSet(6, 200, 56, 49), 
                                                   new KeyByteSet(7, 89, 45,142), 
                                                   new KeyByteSet(8, 6, 88, 32)
                                               };

            var pkvLicenceKeyGenerator = new PkvLicenceKeyGenerator();

            string licenceKey = pkvLicenceKeyGenerator.MakeKey(1, keyByteSets);

            using (var outputStream = new FileStream(@"C:\SVN\LicenceEngine\LicenceEngineV2\AppSoftware.LicenceEngine.Tests\TestActivationKeyFiles\AppSoftware.LicenceEngine.Activation.xml", FileMode.Create))
            {
                activationData.WriteActivationKeyFileXml("7sda2ajfdg56", outputStream, licenceKey);

                outputStream.Close();
            }
        }

    }
}

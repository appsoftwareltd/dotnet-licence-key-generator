# .NET Licence Key Generator #

## Key Generation and Verification System component for Microsoft .NET applications (.NET 2.0 and above) ##

A common requirement for software vendors is the inclusion of a system for generating and verifying passwords, which can be bundled with distributed desktop software, or for granting access to online services. App Software's .NET Licence Engine SDK provides a robust, lightweight and tested means of solving this problem.

The .NET Licence Engine SDK implements a 'Partial Number Verification System', which along with other features, provides a defence against key generators which may be built to attempt to crack your licence key system.

### Key benefits of implementing your licence system with the .NET Licence Key Generator are: ###

No 'Phone Home' to verify licence keys is required.
Ability to revoke licence keys if keys found to have been distributed without authorisation.
Compiled key verification code cannot be fully reproduced to allow the creation of a key generator (keygen).
Keys can be bound to a particular product version if required.

### Additional benefits: ###

Light weight and fast code execution, tested up to 1,000,000 key generation and verification cycles in 10.2 seconds.
Simple to use SDK and sample projects included.
30 day free trial available.

### How it works ###

**(Extended documentation below)**

To generate a licence key we essentially need two things:

A set of arbitrary 'key' bytes that are unique to the application that we want to protect. You are responsible for choosing a random set of bytes to use as your 'keys'
A seed that is unique to the context in which a key is being generated, e.g. 1 licence key for 1 user, or 1 licence key for 1 user application id combination.
Together, the seed and the byte keys will control the key that is output. What is unique to this system, is that when validating the licence key, you only test a subset of the total byte keys used to generate the full key. This means that you don't have to distribute all the keys used to create the licence key, and therefore it is harder to create a keygen to crack the keys you generate. A cracker does not have all the information to reverse engineer your key verification system.

With this system, you are able to vary the keys tested on new builds for published versions of your software, and you have the option of generating new byte key sets for new versions.

You can vary the number of byte keys used to make longer, stronger keys. Your validation code will need to know how many keys were used in the generation of a key.

.NET Licence Engine has been vigorously tested to provide you with a straight forward means of implementing strong licence key generation and verification functionality into your software.

You can download now and use on a 30 day trial basis and integrate .NET Licence Engine into your application today.

You should be aware that no distributed software can be protected 100% against crackers. Whatever the technique used to generate a licence key system, all distributed software has the potential to be decompiled and modified to that licence key verification is skipped entirely. Depending on the context in which you distribute your software, you may need to employ obfuscation and other security techniques to make this less likely. Our system provides a simple programming interface, that helps to create a user friendly, cracker resistant means of implementing licence key functionality in your application.

## Terms of use ##

App Software Ltd and our partners accept no liability for any loss or damage to physical property or intellectual property arising as a result of using our products.

Users of our software, licenced or otherwise are expected to undertake their own thorough testing and assessment for suitability of our products before integrating with their solutions.


# Documentation #

## 1. An Introduction to .NET Licence Engine ##

.NET Licence Engine is an SDK that provides a simple and flexible Partial Key Verification implementation.

Simply put, .NET Licence Engine provides a mechanism for creating and verifying licence keys in .NET applications, without requiring the client application to contact the licence server for verification of keys.

The system is cracker resistant, as it does not require the client to verify the entire key, only a portion. It is impossible for an attacker to build a key generator from decompiling your executable code alone.

## 2. Including .NET Licence Engine in your Projects ##

This wiki contains full documentation for use of .NET Licence Engine, however the interface is small, so you may want to jump straight to a simple client server example using the SDK.

To use .NET Licence Engine in your projects, you will need a means of generating user keys, and a means of allowing a user to input their key for verification.

This might include:

1. A server component (e.g. a section on your website where a user purchases software, that generates a licence key for a user).

2. A client component (e.g. a section on your distributable application that allows a user to enter a key before the application will activate for use).

The server component here is required for generating licence keys. This area of the application will require references to:

**AppSoftware.LicenceEngine.KeyGenerator.dll**

**AppSoftware.LicenceEngine.Common.dll**

Referencing these dlls will enable your server component to use insatnces of PkvLicenceKeyGenerator to generate licence keys.

The key method on this class has the signature:

    public string MakeKey(int seed, KeyByteSet[] keyByteSets)

It is this method that you will use to generate your licence keys.

The seed argument is what links a generated key to a given user or other entity. If you were simply to use a user id, the same key would always be generated for that user (providing the same values are provided for the argument keyByteSets). A low integer seed value will produce a licence key string with leading zeros, so you may want to add a constant to the seed for generation and verification of keys if this is not desirable.

The `keyByteSets` argument to this method is an array of KeyByteSet objects for which you randomly pick byte values that create they key by which your licence keys are generated. These byte values need not change, until such a time that you want to cause old keys not to validate for subsequent releases of your software (e.g. a new major version release). If you require the ability to be able to create keys for old product versions, you may want to store these byte values in a database and implement logic for generating an appropriate key based on product version. Note that the longer the KeyByteSet array, the stronger the key, and the longer the licence key string generated.

The client component here is required for validating licence keys. This area of the application will require references to:

**AppSoftware.LicenceEngine.KeyVerification.dll**

**AppSoftware.LicenceEngine.Common.dll**

Note that it is important that you do not include AppSoftware.LicenceEngine.KeyGenerator.dll with your distributables, or the full bytes used for keyByteSets in key generation. Inclusion of either may make it easier for an attacker to create a keygen for your licence key generation system.

Referencing these dlls will enable your server component to use insatnces of `PkvKeyCheck` to validate licence keys.

The key method on this class has the signature:

    public PkvLicenceKeyResult CheckKey(string key, KeyByteSet[] keyByteSetsToCheck, int totalKeyByteSets, string[] blackListedSeeds)

The `key` argument is the key as provided by the user. Handling of special characters and letter casing will be handled by this method.

The `keyByteSetsToCheck` argument is the key bytes you have selected to verify. Pass copies of 1 or more (but not all) of the KeyByteSet instances as provided to MakeKey. The ability to verify a key string using only part of the full key is a core feature of the .NET Licence Engine, and means that they full key does not have to be distributed publicly.

The `totalKeyByteSets` should be the same as the length of the full KeyByteSet array used when creating keys.

The `blackListedSeeds` allows you to pass an array of blacklisted seeds should you wish for example to bar any users for using a keys in their possession for future releases of your application.

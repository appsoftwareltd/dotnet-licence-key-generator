using System;

namespace AppSoftware.LicenceEngine.Common
{
    /// <summary>
    /// Exception indicating that the activation key file was not found in the correct location.
    /// </summary>
    public class ActivationKeyFileNotPresentException : Exception
    {
        public ActivationKeyFileNotPresentException(string message): base(message)
        {

        }
    }

    /// <summary>
    /// Exception indicating that the trial period for this application has expired.
    /// </summary>
    public class TrialPeriodExpiredException : Exception
    {
        public TrialPeriodExpiredException(string message) : base(message)
        {
            
        }
    }

    /// <summary>
    /// Exception indicating that value in activation key file was invalid.
    /// </summary>
    public class ActivationKeyInvalidException : Exception
    {
        public ActivationKeyInvalidException(string message) : base (message)
        {
            
        }
    }
}

namespace AccessOperating.Models
{
    public class Enums
    {
        /// <summary>
        /// Used as a verification code in all operations.
        /// Non-zero is considered a failure.
        /// </summary>
        public enum VerifyStatusCode
        {
            /// <summary>
            /// Successful
            /// </summary>
            Success = 0,
            /// <summary>
            /// User not found
            /// </summary>
            NotFound = 1,
            /// <summary>
            /// User has no biometric record
            /// </summary>
            NotEnrolled = 2,
            /// <summary>
            /// User's biometric type is invalid
            /// </summary>
            NotAllowedBioType = 3,
            /// <summary>
            /// User not verified
            /// </summary>
            NotVerified = 4,
            /// <summary>
            /// Card not supported for visitor or user.
            /// </summary>
            CardNotSupported = 5,
        }
        /// <summary>
        /// Access Direction
        /// </summary>
        public enum AccessDirection
        {
            /// <summary>
            /// Exit
            /// </summary>
            Out = 0,
            /// <summary>
            /// Entry
            /// </summary>
            In = 1
        }
    }
}

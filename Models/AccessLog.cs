
using System.Text.Json.Serialization;
using static AccessOperating.Models.Enums;

namespace AccessOperating.Models
{
    // Define AccessLog class based on the response structure: (for demonstration purposes only to this task)
    /*
    {
        "logID": "1691584573860",
        "computerHash": "XPV9G",
        "ipAddress": "222.241.251.254",
        "userID": "92b584a702664b3b8ba4a4f604e6bcdf",
        "username": "Şeyda Nur Arikan",
        "accessLocation": "Yemekhane",
        "accessDirection": 0,
        "verifyStatusCode": 5,
        "additionalInfo": "yLOqnfzUNpzW?YvWJXyfZDuNKpTenpuwnCDGmKNsZlWht?QTAS",
        "logTime": "2010-06-20T00:00:00"
    }
    */
    /// <summary>
    /// Represents an access log containing information about a user's access activity.
    /// </summary>
    public class AccessLog
    {
        /// <summary>
        /// Gets or sets the unique identifier for the access log.
        /// </summary>
        [JsonPropertyName("logID")]
        public string LogID { get; set; }

        /// <summary>
        /// Gets or sets the hash of the computer associated with the access.
        /// </summary>
        [JsonPropertyName("computerHash")]
        public string ComputerHash { get; set; }

        /// <summary>
        /// Gets or sets the IP address from which the access was made.
        /// </summary>
        [JsonPropertyName("ipAddress")]
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the access.
        /// </summary>
        [JsonPropertyName("userID")]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the username associated with the access.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the location where the access was made.
        /// </summary>
        [JsonPropertyName("accessLocation")]
        public string AccessLocation { get; set; }

        /// <summary>
        /// Gets or sets the direction of the access (In or Out).
        /// </summary>
        [JsonPropertyName("accessDirection")]
        public AccessDirection AccessDirection { get; set; }

        /// <summary>
        /// Gets or sets the verification status code for the access.
        /// </summary>
        [JsonPropertyName("verifyStatusCode")]
        public VerifyStatusCode VerifyStatusCode { get; set; }

        /// <summary>
        /// Gets or sets additional information related to the access.
        /// </summary>
        [JsonPropertyName("additionalInfo")]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the access occurred.
        /// </summary>
        [JsonPropertyName("logTime")]
        public DateTime LogTime { get; set; }
    }

}

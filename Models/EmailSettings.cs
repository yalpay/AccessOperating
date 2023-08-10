namespace AccessOperating.Models
{
    /// <summary>
    /// Represents SMTP configuration settings for sending emails.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the SMTP server address.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets the SMTP port number.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the username for authenticating with the SMTP server.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for authenticating with the SMTP server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the email address of the recipient.
        /// </summary>
        public string ToAddress { get; set; }
    }

}

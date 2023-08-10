using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using AccessOperating.Models;

namespace AccessOperating.Helpers
{
    public class SmtpHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
       string lpAppName,
       string lpKeyName,
       string lpDefault,
       StringBuilder lpReturnedString,
       uint nSize,
       string lpFileName);

        /// <summary>
        /// Reads SMTP settings from the configuration file.
        /// </summary>
        /// <returns>EmailSettings object with SMTP configuration.</returns>
        public static EmailSettings ReadSmtpSettings(string filename)
        {
            if (!File.Exists(filename))
            {
                // Handle the case where the Settings.ini file does not exist
                throw new FileNotFoundException("The Settings.ini file was not found.");
            }

            StringBuilder sb = new StringBuilder(255);
            var lpAppName = "SMTP";

            GetPrivateProfileString(lpAppName, "Server", "", sb, (uint)sb.Capacity, filename);
            string smtpServer = sb.ToString();
            if (string.IsNullOrEmpty(smtpServer))
            {
                throw new ArgumentException("The SmtpServer value is not valid.");
            }

            GetPrivateProfileString(lpAppName, "Port", "0", sb, (uint)sb.Capacity, filename);
            int smtpPort = int.Parse(sb.ToString());
            if (smtpPort <= 0 || smtpPort > 65535)
            {
                throw new ArgumentException("The SmtpPort value is not valid.");
            }

            GetPrivateProfileString(lpAppName, "Username", "", sb, (uint)sb.Capacity, filename);
            string username = sb.ToString();
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("The Username value is not valid.");
            }

            GetPrivateProfileString(lpAppName, "Password", "", sb, (uint)sb.Capacity, filename);
            string password = sb.ToString();
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("The Password value is not valid.");
            }

            GetPrivateProfileString(lpAppName, "FromAddress", "", sb, (uint)sb.Capacity, filename);
            string fromAddress = sb.ToString();
            if (string.IsNullOrEmpty(fromAddress))
            {
                throw new ArgumentException("The FromAddress value is not valid.");
            }

            GetPrivateProfileString(lpAppName, "ToAddress", "", sb, (uint)sb.Capacity, filename);
            string toAddress = sb.ToString();
            if (string.IsNullOrEmpty(toAddress))
            {
                throw new ArgumentException("The ToAddress value is not valid.");
            }

            return new EmailSettings
            {
                SmtpServer = smtpServer,
                SmtpPort = smtpPort,
                Username = username,
                Password = password,
                FromAddress = fromAddress,
                ToAddress = toAddress
            };
        }

        /// <summary>
        /// Sends an email using SMTP settings and log information.
        /// </summary>
        /// <param name="smtpSettings">SMTP configuration.</param>
        /// <param name="log">AccessLog object for which to send an email.</param>
        public static void SendEmail(EmailSettings smtpSettings, AccessLog log)
        {
            using var client = new SmtpClient(smtpSettings.SmtpServer, smtpSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
                EnableSsl = true
            };

            var message = new MailMessage(smtpSettings.FromAddress, smtpSettings.ToAddress)
            {
                Subject = "Invalid Access Log",
                Body = $"Invalid access log: LogID: {log.LogID}, Username: {log.Username}, Verify Status: {log.VerifyStatusCode}"
            };

            client.Send(message);
        }
    }
}

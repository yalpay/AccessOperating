// Using statements
using NLog;
using static AccessOperating.Helpers.ApiHelper;
using static AccessOperating.UserInterface;
using static AccessOperating.Helpers.SmtpHelper;
using AccessOperating.Models;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;


namespace AccessOperating
{
    public class Program
    {        
        public static async Task Main(string[] args)
        {

            // initialize configuration
            var dir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("Settings/appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var logger = LogManager.Setup()
                                   .SetupExtensions(ext => ext.RegisterConfigSettings(configuration))
                                   .GetCurrentClassLogger();

            try
            {
                logger.Info("Application started");

                string apiUrl = configuration["ApiUrl"];                

                // Perform GET request to access log endpoint
                string responseJson = PerformGetRequest(apiUrl);
                logger.Info("Access logs fetched from API endpoint");

                // Parse JSON and store logs in Custom Linked List
                CustomLinkedList<AccessLog> accessLogs = ParseAndStoreLogs(responseJson);
                if (accessLogs == null)
                {
                    logger.Info("There is no logs on the API endpoint");
                    Console.WriteLine("There is no logs");
                    return;
                } else
                {
                    logger.Info("Access logs json data parsed and stored");
                }
                
                // Display logs on a grid
                DisplayLogs(accessLogs);
                logger.Info("Access logs displayed on grid");

                // Check for VerifyStatusCode and show pop-up if needed
                var current = accessLogs.GetHead();
                while (current != null)
                {
                    var log = current.Value;
                    if (log.VerifyStatusCode > 0)
                    {
                        int timeout = int.Parse(configuration["OperatorConfirmationTimeout"]);
                        bool operatorConfirmed = await WaitForOperatorConfirmationAsync(timeout);

                        if (operatorConfirmed)
                        {
                            // Perform POST request to acknowledge the access log
                            PerformPostRequest(log, apiUrl);
                            logger.Info($"Log ID: {log.LogID} is confirmed and posted to the API");
                        }
                        else
                        {
                            string settingsFile = configuration["SettingsFile"];
                            // Read SMTP settings and send email
                            EmailSettings smtpSettings = ReadSmtpSettings(settingsFile);
                            SendEmail(smtpSettings, log);
                            logger.Info($"Log ID: {log.LogID} is not confirmed by the operator and sent email");
                        }
                    } else
                    {
                        // No specified
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred: {ErrorMessage}", ex.Message);
            }
        }
    }
}

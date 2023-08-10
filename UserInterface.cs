using AccessOperating.Models;
using Newtonsoft.Json.Linq;
using NLog.Fluent;

namespace AccessOperating
{
    public class UserInterface
    {
        /// <summary>
        /// Parses JSON response and stores logs in a linked list.
        /// </summary>
        /// <param name="responseJson">JSON response from the API.</param>
        /// <returns>Linked list of AccessLog objects.</returns>
        public static CustomLinkedList<AccessLog>? ParseAndStoreLogs(string responseJson)
        {
            if (String.IsNullOrEmpty(responseJson))
            {
                return null;
            }

            JToken token = JToken.Parse(responseJson);
            AccessLog[] logs;

            if (token.Type == JTokenType.Array)
            {
                logs = token.ToObject<AccessLog[]>();
            }
            else
            {
                logs = new AccessLog[] { token.ToObject<AccessLog>() };
            }

            var accessLogs = new CustomLinkedList<AccessLog>();

            for(int i = 0; i < logs.Length; i++)
            {
                accessLogs.AddLast(logs[i]);
            }

            return accessLogs;
        }



        /// <summary>
        /// Displays logs from the linked list.
        /// </summary>
        /// <param name="accessLogs">Linked list of AccessLog objects.</param>
        public static void DisplayLogs(CustomLinkedList<AccessLog> accessLogs)
        {
            if(accessLogs == null)
            {
                Console.WriteLine("There is no logs");
                return;
            }

            var current = accessLogs.GetHead();

            while (current != null)
            {
                var log = current.Value;
                Console.WriteLine($"Pop-up: Log ID: {log.LogID}, Username: {log.Username}, Verify Status: {log.VerifyStatusCode}, Additional Info: {log.AdditionalInfo}");
                current = current.Next;
            }
        }

        /// <summary>
        /// Waits for operator confirmation within 30 seconds.
        /// </summary>
        /// <returns>True if operator confirmed, false if not.</returns>
        public static async Task<bool> WaitForOperatorConfirmationAsync(int timeout)
        {
            bool userConfirmation = false;
            Console.WriteLine("Type ok for operator confirmation...");

            // TODO: implement the actual logic for confirmation
            Timer timer = new Timer(TimerElapsed, null, timeout, Timeout.Infinite);
            string input = Console.ReadLine();
            timer.Dispose();

            if (input == "ok")
            {
                userConfirmation = true;
            }

            return userConfirmation;
        }

        public static void TimerElapsed(object state)
        {
            Console.WriteLine("Time's up!");
            Environment.Exit(0);
        }
    }
}

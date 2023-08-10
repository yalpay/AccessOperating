using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using AccessOperating.Models;

namespace AccessOperating.Helpers
{
    public class ApiHelper
    {
        /// <summary>
        /// Performs a GET request using the specified API URL.
        /// </summary>
        /// <param name="apiUrl">URL of the API endpoint.</param>
        /// <returns>Response content as a string.</returns>
        public static string PerformGetRequest(string apiUrl)
        {
            Uri uri = new Uri(apiUrl);
            string host = uri.Host;
            int port = uri.Port != -1 ? uri.Port : 80;

            using (TcpClient client = new TcpClient(host, port))
            using (NetworkStream stream = client.GetStream())
            {
                string request = $"GET {uri.PathAndQuery} HTTP/1.1\r\n" +
                                 $"Host: {host}\r\n" +
                                 "Connection: close\r\n" +
                                 "\r\n";

                byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                stream.Write(requestBytes, 0, requestBytes.Length);

                StringBuilder responseBuilder = new StringBuilder();
                bool readingContent = false;

                using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            readingContent = true;
                            continue;
                        }

                        if (readingContent)
                        {
                            responseBuilder.AppendLine(line);
                        }
                    }
                }

                return responseBuilder.ToString();
            }
        }

        /// <summary>
        /// Performs a GET request using the specified API URL asynchronously.
        /// </summary>
        /// <param name="apiUrl">URL of the API endpoint.</param>
        /// <returns>Response content as a string.</returns>
        public static async Task<string> PerformGetRequestAsync(string apiUrl)
        {
            Uri uri = new Uri(apiUrl);
            string host = uri.Host;
            int port = uri.Port != -1 ? uri.Port : 80;

            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(host, port);

                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII, bufferSize: 1024, leaveOpen: true))
                using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string request = $"GET {uri.PathAndQuery} HTTP/1.1\r\n" +
                                     $"Host: {host}\r\n" +
                                     "Connection: close\r\n" +
                                     "\r\n";

                    await writer.WriteAsync(request);
                    await writer.FlushAsync();

                    StringBuilder responseBuilder = new StringBuilder();
                    bool readingContent = false;

                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            readingContent = true;
                            continue;
                        }

                        if (readingContent)
                        {
                            responseBuilder.AppendLine(line);
                        }
                    }

                    return responseBuilder.ToString();
                }
            }
        }


        /// <summary>
        /// Performs a POST request to acknowledge an access log.
        /// </summary>
        /// <param name="log">AccessLog object to acknowledge.</param>
        /// <param name="apiUrl">URL of the API endpoint.</param>
        public static void PerformPostRequest(AccessLog log, string apiUrl)
        {
            Uri uri = new Uri(apiUrl);
            string postData = JsonSerializer.Serialize(new { log.LogID, Description = "Log acknowledged" });
            string request = $"POST {uri.PathAndQuery} HTTP/1.1\r\nHost: {uri.Host}\r\nContent-Type: application/json\r\nContent-Length: {Encoding.ASCII.GetBytes(postData).Length}\r\nConnection: close\r\n\r\n{postData}";

            using (Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                socket.ConnectAsync(uri.Host, 80);

                byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                socket.SendAsync(new ArraySegment<byte>(requestBytes), SocketFlags.None);

                byte[] responseBuffer = new byte[4096];
                int bytesRead = socket.Receive(new ArraySegment<byte>(responseBuffer), SocketFlags.None);
                string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

                // You can handle the response here if needed
            }
        }

        /// <summary>
        /// Performs a POST request to acknowledge an access log asynchronously.
        /// </summary>
        /// <param name="log">AccessLog object to acknowledge.</param>
        /// <param name="apiUrl">URL of the API endpoint.</param>
        public static async Task PerformPostRequestAsync(AccessLog log, string apiUrl)
        {
            Uri uri = new Uri(apiUrl);
            string postData = JsonSerializer.Serialize(new { log.LogID, Description = "Log acknowledged" });
            string request = $"POST {uri.PathAndQuery} HTTP/1.1\r\nHost: {uri.Host}\r\nContent-Type: application/json\r\nContent-Length: {Encoding.ASCII.GetBytes(postData).Length}\r\nConnection: close\r\n\r\n{postData}";

            using (Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                await socket.ConnectAsync(uri.Host, 80);

                byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                await socket.SendAsync(new ArraySegment<byte>(requestBytes), SocketFlags.None);

                byte[] responseBuffer = new byte[4096];
                int bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), SocketFlags.None);
                string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

                // You can parse the response here if needed
            }
        }
    }
}

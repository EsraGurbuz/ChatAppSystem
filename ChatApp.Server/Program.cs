using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChatApp.Shared;
using Newtonsoft.Json;

namespace ChatApp.Server
{
    class Program
    {
        // Thread-safe list to keep track of connected clients
        private static List<TcpClient> clients = new List<TcpClient>();

        static async Task Main(string[] args)
        {
            // Set the IP address and Port
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Server started on port 5000...");

            while (true)
            {
                // Accept incoming connection requests asynchronously
                TcpClient client = await listener.AcceptTcpClientAsync();
                lock (clients) { clients.Add(client); }

                Console.WriteLine("New client connected!");

                // Handle each client in a separate Task (Fire and Forget)
                _ = HandleClientAsync(client);
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    // Blocking avoided by using ReadAsync
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Client disconnected

                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var message = JsonConvert.DeserializeObject<ChatMessage>(jsonMessage);

                    Console.WriteLine($"[{message.Timestamp}] {message.Sender}: {message.Message}");

                    // Broadcast the message to everyone
                    await BroadcastMessage(jsonMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                lock (clients) { clients.Remove(client); }
                client.Close();
            }
        }

        private static async Task BroadcastMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                await client.GetStream().WriteAsync(data, 0, data.Length);
            }
        }
    }
}
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            TcpListener server = new TcpListener(IPAddress.Any, 33533);
            server.Start();
            Console.WriteLine("LMG Lernzeiten Server APplication Lise-Meitner Gymnasium Leverkusen");
            Console.WriteLine("Server starting");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connection incomming...");
                ThreadPool.QueueUserWorkItem(HandleClient, client);
            }
        }catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
    static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        Console.WriteLine($"Connection established with {client.Client.RemoteEndPoint}");
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received message:\r\n" + message);

                string response = HandleMessage(message);
                Console.WriteLine("Responding with:\r\n");
                Console.WriteLine(message);
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Client error: " + e.Message);
            Console.ResetColor();
        }
        finally
        {
            client.Close();
        }
    }
    static string HandleMessage(string message)
    {
        if (message == "Hello World!")
        {
            return "Hello!";
        }
        else
        {
            return "0";
        }
    }
}
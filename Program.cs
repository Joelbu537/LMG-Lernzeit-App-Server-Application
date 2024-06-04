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
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received message:\r\n" + message);
                string[] content = message.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string response = HandleMessage(content);
                Console.WriteLine("Responding with:");
                Console.WriteLine(response.Replace("\r\n", " "));
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
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
    static string HandleMessage(string[] content)
    {
        string email = "";
        string hash = "";
        switch (content[0])
        {
            case "login":
                email = content[1];
                hash = content[2];
                //CHECK DB
                int status = 1;//DEBUG
                return $"login\r\n{status}";
            case "getmods":

                return "getmods\r";
            default:
                return "-1";
        }
    }
}
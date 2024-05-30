using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static string Version = "0.0.3";
    static async void Main(string[] args)
    {
        if(args.Length != 0)
        {
            //Handle parameters
        }
        while (true)
        {
            try
            {
                Console.WriteLine("Lise Meinter Gymnasium Leverkusen Lernzeit-App Server Application " + Version);
                Console.WriteLine("Starting tcplistener...");
                TcpListener listener = new TcpListener(IPAddress.Any, 33533);
                listener.Start();
                Console.WriteLine("Starting tcpclient");
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Incoming connection...");
                try
                {
                    Task.Run(() => HandleClient(client));
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Connection could not be established!");
                    Console.WriteLine(ex.ToString());
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }
    static void HandleClient(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            Console.WriteLine($"Connected to {client.Client.RemoteEndPoint}!");
            byte[] buffer = new byte[1024];
            int bytesRead;
            try
            {
                Thread.Sleep(300);
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                Console.WriteLine($"Received stream content ({bytesRead})");
                string datareceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string[] content = datareceived.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                string reply = "";
                byte[] reply_bytes;

                switch (content[0])
                {
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Unknown request sent by {client.Client.RemoteEndPoint}");
                        Console.ResetColor();
                        stream.Close();
                        client.Close();
                        break;
                    case "verify":
                        int status = Verify(content[1], content[2]);
                        reply = $"verify\r\n{status}";
                        reply_bytes = Encoding.UTF8.GetBytes(reply);
                        stream.Write(reply_bytes, 0, reply_bytes.Length);
                        Thread.Sleep(300);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{client.Client.RemoteEndPoint} ran into a critical error");
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }
    }
    static int Verify(string email, string hash)
    {
        //Datenbankzeug
        return 1;
    }
}
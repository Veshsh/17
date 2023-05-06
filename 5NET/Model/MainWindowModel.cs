using _5NET.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
namespace _5NET.Model
{
    public static class TcpServer
    {
        private static Socket socket=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> clients = new List<Socket>();
        private static HashSet<string> Log=new HashSet<string>();
        public static void ServerStart(EnterModel enter)
        {
            IPEndPoint iPPoint = new IPEndPoint(IPAddress.Parse(enter.Ip.Split(':')[0]), Convert.ToInt32(enter.Ip.Split(':')[1]));
            socket.Bind(iPPoint);
            socket.Listen(2);
            _ = ListenToClient();
        }
        private static async Task ListenToClient()
        {
            while (true)
            {
                Socket client = await socket.AcceptAsync();
                clients.Add(client);
                _ = RecieveMessage(client);
            }
        }
        private static async Task RecieveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                Log.Add($"{client.RemoteEndPoint}");
                MainViewModel2.Log.Clear();
                foreach (string log in Log)
                    MainViewModel2.Log.Add(log);
                foreach (Socket item in clients)
                    _ = SendMessage(item, message); 
            }
        }
        private static async Task SendMessage(Socket client,string message)
        {
            byte[] bytes =Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes,SocketFlags.None);
        }
    }
    public static class TcpClient
    {
        private static string name="noname";
        private static Socket server= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static HashSet<string> names = new HashSet<string>();
        public static void ClientStart(EnterModel enter)
        {
            name = enter.Name;
            try
            {
                server.ConnectAsync(enter.Ip.Split(':')[0], Convert.ToInt32(enter.Ip.Split(':')[1]));
            }
            catch 
            {
                MessageBox.Show("ERROR Program Shotdown Auto");
                Application.Current.Shutdown();
            }
            _ = RecieveMessage();
        }
        private static async Task RecieveMessage()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                if (message.Split(": ")[2] == "/disconnect")
                    names.Remove(message.Split(": ")[1]);
                else
                {
                    names.Add(message.Split(": ")[1]);
                    MainViewModel1.MessageStory.Add(message);
                    MainViewModel2.MessageStory.Add(message);
                }
                MainViewModel1.Clients.Clear();
                MainViewModel2.Clients.Clear();
                foreach (string name in names)
                {
                    MainViewModel1.Clients.Add(name);
                    MainViewModel2.Clients.Add(name);
                }
            }
        }
        public static async Task SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(MessageFormat(message));
            await server.SendAsync(bytes, SocketFlags.None);
        }
        private static string MessageFormat(string message)
        {
            return DateTime.Now+": "+name+": "+message+": ";
        }
    }

    public class EnterModel
    {
        public EnterModel(string Ip,string Name) {
            this.Name = Name;
            this.Ip = Ip;
        }
        public string Ip { get; set; }
        public string Name { get; set; }
    }
}

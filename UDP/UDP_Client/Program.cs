using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpClientApp
{
    class Program
    {
        static int Port_Reciever;
        static int Port_Connection;
        static string LocalIp;
        static void Main(string[] args)
        {
            try
            {
                LocalIp = GetLocalIP();
                Console.WriteLine("Введите порт для прослушивания: ");
                Port_Reciever = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Введите порт для подключения: " + Port_Connection);
                Port_Connection = Int32.Parse(Console.ReadLine()); // порт, к которому мы подключаемся
                Console.WriteLine("Локальный адрес: " + LocalIp);
                Thread receiveThread = new Thread(new ThreadStart(Receive));
                receiveThread.Start();
                Send(); // отправляем сообщение
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static private string GetLocalIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }
        private static void Send()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки сообщений
            try
            {
                while (true)
                {
                    string message = Console.ReadLine(); // сообщение для отправки
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, LocalIp, Port_Connection); // отправка
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private static void Receive()
        {
            UdpClient receiver = new UdpClient(Port_Reciever); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine("Ответ: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
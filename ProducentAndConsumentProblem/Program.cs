using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ProducentAndConsumentProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketFun();
        }

        public static void SocketFun()
        {
            int port = 22345;

            var tcpListener = new TcpListener(IPAddress.Any, port);

            tcpListener.Start();

            new Thread(() =>
            {

                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - czeka na połączenie z portem");

                var socket = tcpListener.AcceptSocket();

                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - Połączenie zaakceptowane");

                var stream = new NetworkStream(socket);
                var reader = new System.IO.BinaryReader(stream);

                try
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - Zaczyna blokowanie czytania");
                    var bytes = reader.ReadBytes(1);
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - Skończone blokowanie czytania, przeczytano {0} bytes", bytes.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("błąd czytania: " + ex);
                }
            }).Start();

            new Thread(() =>
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Console.WriteLine("\t" + Thread.CurrentThread.ManagedThreadId + " - Połączenie z portem lokalnym");

                socket.Connect("127.0.0.1", port);

                Console.WriteLine("\t" + Thread.CurrentThread.ManagedThreadId + " - Połączenie z portem lokalnym się udało");

                Thread.Sleep(TimeSpan.FromSeconds(2));

                Console.WriteLine("\t" + Thread.CurrentThread.ManagedThreadId + " - Usuwanie soketu");

                socket.Dispose();

            }).Start();

            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}

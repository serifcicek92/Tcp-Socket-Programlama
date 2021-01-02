using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpSocketProgramlama
{
    class Program
    {
        static Socket soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //soket tanımlıyoruz
        const int PORT = 52000;//port tanımlıyoruz
        const string uzakClientIP = "192.168.0.80";
        static void Main(string[] args)
        {
            try
            {
                soket.Connect(new IPEndPoint(IPAddress.Parse(uzakClientIP), PORT));//bağlantı sağlama
                Console.WriteLine("Başarıyla bağlanıldı!");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n (X) -> Bağlanmaya çalışırken hata oluştu: " + e.Message);
            }

            while (true && soket.Connected)
            {
                Console.Write("Gönder: ");

                // Gönderilecek metni alıyoruz.
                string gonder = Console.ReadLine();

                // Ağ üzerinden gönderilecek her şey bytelara 
                // dönüştürülmüş olmalıdır.
                soket.Send(Encoding.UTF8.GetBytes(gonder));
            }

            //string GonderilecekMesaj = Console.ReadLine();
            //soket.Send(Encoding.UTF8.GetBytes(GonderilecekMesaj));
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Programı sonlandır");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}

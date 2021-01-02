using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace client
{
    class Program
    {
        static Socket dinleyiciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 52000;
        static void Main(string[] args)
        {
            Console.Title = "TCP Data Transmit - Dinleyici";

            // TCP dinleyicisini herhangi bir IPAdresinden gelebilecek
            // bağlantı tekliflerini dinleyecek şekilde ayarlıyoruz.
            // Ayrıca gönderici ile aynı PORT üzerinden dinlemesi gerekiyor.
            TcpListener dinle = new TcpListener(IPAddress.Any, PORT);
            dinle.Start();

            Console.WriteLine("Bağlantı bekleniyor...");

            // Buradan aşağısı bağlantı teklifi geldiğinde çalışacaktır.
            // Teklifi kabul ediyoruz ve dinleyiciSoketimize atayarak
            // kurulmuş TCP bağlantısına her zaman erişilebilecek hale getiriyoruz.
            dinleyiciSoket = dinle.AcceptSocket();
            Console.WriteLine("Bağlantı sağlandı. ");

            while (true)
            {
                try
                {
                    // Receive() metodu TCP bağlantısı üzerinden gelecek mesajları beklemeye ve
                    // geldiğinde okumaya yarar. Gelecek mesajı içinde saklayacağımız bir
                    // buffer yaratarak (gelenData) gelen byteları depoluyoruz.
                    // Oluşturduğumuz buffer'ın boyutu tek seferde alınabilecek byte sayısını
                    // belirlediği için, 256 byte'tan daha uzun bir data gönderildiğinde
                    // tamamını alamayacaktır. Daha uzun veriler göndermek istiyorsanız buffer 
                    // boyutunu daha büyük belirtebilirsiniz.
                    byte[] gelenData = new byte[256];
                    dinleyiciSoket.Receive(gelenData);

                    // Gelen karakterler yarrattığımız bufferın tamamını dolduramazsa (ki genelde
                    // doldurmayacaktır) kalan karakterler boşluk olarak gözükür. Bu sebeple Split()
                    // metodunu kullanarak gelen mesajın sadece metnin bittiği noktaya kadar alınmasını
                    // sağlıyoruz.
                    string mesaj = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];

                    Console.WriteLine("Gelen mesaj: " + mesaj);

                    if (mesaj.ToLower().StartsWith("exit"))
                    {
                        // Eğer mesaj 'exit' şeklinde başlıyorsa, bağlantıyı düzgün bir şekilde kapatıyoruz.
                        Console.WriteLine("Bağlantı kapatılıyor.");
                        dinleyiciSoket.Shutdown(SocketShutdown.Both);
                        dinleyiciSoket.Close();
                      
                        break;
                    }
                }
                catch
                {
                    // Eğer gönderici program TCP bağlantısı kurulduktan sonra, düzgün bir şekilde
                    // yani exit komutu verilmeden kapatılırsa bir hata oluşacaktır. Hatayı burda
                    // ele alarak programın çökmesini engelliyoruz.
                    Console.WriteLine("Bağlantı kesildi. Çıkış yapılıyor.");
                    break;
                }
            }

            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}

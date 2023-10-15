using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Channels;
using System.IO;
using System.Net.NetworkInformation;

namespace ConsoleApp122_2023_09_28
{
    public class Network
    {
        public IPAddress ipAdress;
        public TcpListener listener;
        private TcpClient DrugoiClient;

        private TcpClient SvoiClient;

        private NetworkStream DrugoiClientNetwork;
        private NetworkStream SvoiClientNetwork;


        public async void GetOtpravkaAsync()
        {
            ipAdress = IpAddressa(IP.MOI)[0];
            listener = new TcpListener(ipAdress, 1050);
            listener.Start();
            Console.WriteLine("Сервер күтіп тұр");
            DrugoiClient = await listener.AcceptTcpClientAsync();
            DrugoiClientNetwork = DrugoiClient.GetStream();
            Console.WriteLine("Сервер қосылды");
        }


        public void GetPriniat(string IP)
        {
            SvoiClient = new TcpClient(IP, 1050);
            SvoiClientNetwork = SvoiClient.GetStream();
            Console.WriteLine("Клиент қосылды");

        }






        public List<List<int>> zhdatOtvetNaparni()
        {
            List<List<int>> data = default(List<List<int>>);
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = SvoiClientNetwork.Read(buffer, 0, buffer.Length);
                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                data = JsonConvert.DeserializeObject<List<List<int>>>(json);
                data.Reverse();
                for (int i = 0; i < 8; i++)
                {
                    data[i].Reverse();
                }
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (data[i][j] == 1)
                        {
                            data[i][j] = 2;
                        }
                        else if (data[i][j] == 2)
                        {
                            data[i][j] = 1;
                        }

                    }
                }
                Program.pole = data;
                Program.tandau();

            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Ошибка десериализации данных. " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            catch (IOException)
            {
                Console.WriteLine("Соединение с сервером разорвано.");

            }
            return data;


        }


        public void otpravitOtvetNaparniku<T>(T data)
        {

            string json = JsonConvert.SerializeObject(data);
            byte[] message = Encoding.UTF8.GetBytes(json);
            DrugoiClientNetwork.Write(message, 0, message.Length);

        }


        public enum IP { MOI, NeMOI }

        public static List<IPAddress> IpAddressa(IP vybor)
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<IPAddress> ips = new List<IPAddress>();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // Проверяем, является ли интерфейс активным и имеет ли он тип Wi-Fi
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // Получаем IP-адреса для этого интерфейса
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                    UnicastIPAddressInformationCollection ipAddresses = ipProperties.UnicastAddresses;

                    foreach (UnicastIPAddressInformation ipAddress in ipAddresses)
                    {
                        if (ipAddress.Address.ToString().Split(".").Length == 4)
                            ips.Add(ipAddress.Address);
                    }
                }
            }
            return ips;

        }
    }
}
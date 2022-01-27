using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel
{
    public class Connexion
    {
        private static Socket socket;
        private static byte[] buffer = null;
        private static bool isSendingData = false;
        private static bool isReceivingData = false;
        private static string receivedData = null;

        public async Task MainConnexion()
        {
            string[] args = Environment.GetCommandLineArgs();
            // Lecture des paramètres en cours
            if (args == null || args.Length != 3 || !int.TryParse(args[2], out int port))
            {
                string file = typeof(Connexion).Assembly.Location;
                string appExeName = System.IO.Path.GetFileNameWithoutExtension(file);
                Console.WriteLine("Invalid parameters !");
                Console.WriteLine("program usage : " + appExeName + " server_address connection_port");
                return;
            }
            Console.WriteLine("Trying to open connection to server " + args[1] + " on the port " + args[2] + "...");

            OpenConnection(args[1], port);
            if (socket == null)
                return;
            Console.WriteLine("Connection to server opened successfully !");
            return;
            string command;

            /*
            while (true)
            {
                Console.WriteLine("");
                Console.Write(args[1] + "> ");
                command = "DATE";

                // Traitement de la chaîne lue
                if (string.IsNullOrEmpty(command))
                    continue;
                else if (command == "EXIT")
                    break;

                // Envoi de la commande au serveur
                Console.Write("Sending command to server in progress...");
                if (!await BeginSendAsync(command))
                    continue;
                // Attente de la fin de l'envoi
                while (isSendingData)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }
                Console.WriteLine("");
                Console.WriteLine("Command sent to server successfully.");

                // Lecture de la réponse du serveur
                Console.Write("Reading server response in progress...");
                if (!await BeginReceiveAsync())
                    continue;
                // Attente de la fin de la réception
                while (isReceivingData)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }
                Console.WriteLine("");

                // Traitement du résultat lu sur la socket
                if (receivedData == "CONNECTION_CLOSED")
                {
                    Console.WriteLine("Server has closed connection !");
                    break;
                }
                else
                {
                    // Affichage du message
                    Console.WriteLine("Server response : " + receivedData);
                }
            }

            CloseConnection();
            */
        }

        private static string GetAddress(string serverAddress)
        {
            try
            {
                IPHostEntry iphostentry = Dns.GetHostEntry(serverAddress);
                return iphostentry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error while retreiving server ip from received server address : " + e.Message);
            }

            return "";
        }

        private static void OpenConnection(string serverAddress, int connectionPort)
        {
            IPAddress ip = IPAddress.Parse(GetAddress(serverAddress));
            IPEndPoint ipEnd = new IPEndPoint(ip, connectionPort);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipEnd);
                if (socket.Connected)
                    return;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error while openning connection to server : " + e.Message);
            }
            socket = null;
        }

        private static bool CloseConnection()
        {
            if (socket != null && socket.Connected)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    if (socket.Connected)
                        WriteError("Error while closing socket : ");
                    else
                        return true;
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Error while closing socket : " + e.Message);
                }
            }
            return false;
        }

        private static async Task<bool> BeginReceiveAsync()
        {
            if (socket == null || !socket.Connected || socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
            {
                //La connexion a été clôturée par le serveur ou bien un problème
                //réseau est apparu
                Console.WriteLine("");
                Console.WriteLine("Connection to server has been closed.");
                return false;
            }

            await Task.Run(() => { while (isReceivingData) ; });

            isReceivingData = true;
            receivedData = null;

            //var tWaitForEndingSendingData = Task.Run(() => { while (isSendingData) ; });
            var tWaitForDataAvailable = Task.Run(() => { while (socket.Available == 0) ; });
            _ = Task.Factory.ContinueWhenAll(
                new Task[] { tWaitForDataAvailable }, //{ tWaitForEndingSendingData, tWaitForDataAvailable },
                t =>
                {
                    // Délai d'attente artificiel
                    //Thread.Sleep(4000);

                    // Lecture des données
                    try
                    {
                        buffer = new byte[socket.Available];
                        //Réception des données
                        socket.BeginReceive(buffer, 0, socket.Available, SocketFlags.None, ReceiveCallback, null);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Error while starting receiving data on socket : " + e.Message);
                    }
                });
            return true;
        }
        private static void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int length = socket.EndReceive(AR);
                if (length > 1)
                {
                    receivedData = System.Text.Encoding.UTF8.GetString(buffer).Trim();
                }
                else
                {
                    //Si le nombre de bits reçus est égal à 1
                    //La connexion a été clôturée par le serveur ou bien un problème
                    //réseau est apparu
                    Console.WriteLine("");
                    Console.WriteLine("Connection to server has been closed.");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("");
                Console.WriteLine("Error while ending receiving data on socket : " + e.Message);
            }
            finally
            {
                isReceivingData = false;
            }
        }

        private static async Task<bool> BeginSendAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
                return false;

            if (socket == null || !socket.Connected || !socket.Poll(10, SelectMode.SelectWrite))
            {
                //La connexion a été clôturée par le serveur ou bien un problème
                //réseau est apparu
                Console.WriteLine("");
                Console.WriteLine("Connection to server has been closed.");
                return false;
            }

            await Task.Run(() => { while (isSendingData) ; });

            isSendingData = true;
            // Délai d'attente artificiel
            //Thread.Sleep(4000);

            // Envoi des données
            try
            {
                buffer = System.Text.Encoding.UTF8.GetBytes(message);
                socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
            }
            catch (SocketException e)
            {
                Console.WriteLine("");
                Console.WriteLine("Error while starting sending data on socket : " + e.Message);
            }
            return true;
        }
        private static void SendCallback(IAsyncResult AR)
        {
            try
            {
                socket.EndSend(AR);
            }
            catch (SocketException e)
            {
                Console.WriteLine("");
                Console.WriteLine("Error while ending sending data on socket : " + e.Message);
            }
            finally
            {
                isSendingData = false;
            }
        }

        private static void WriteError(string description)
        {
            Console.WriteLine(description + Convert.ToString(System.Runtime.InteropServices.Marshal.GetLastWin32Error()));
        }


        public async Task<string> getMessageAsync()
        {

           // Thread.Sleep(4000);
            bool levier = true;
            while (levier)
            {
                // Lecture de la réponse du serveur
                if (!await BeginReceiveAsync())
                    continue;
                // Attente de la fin de la réception
                while (isReceivingData)
                {
                }
                levier = false;
            }
            return receivedData;
        }

        public async Task<bool> setMessageAsync(string command)
        {
            if (socket == null)
                return false;
            while (true)
            {
                // Lecture de la réponse du serveur
                if (!await BeginSendAsync(command))
                    continue;
                // Attente de la fin de la réception
                while (isSendingData)
                {
                }
                return true;
            }
        }
    }
}


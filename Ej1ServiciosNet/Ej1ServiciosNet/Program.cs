using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ej1ServiciosNet
{
    class Program
    {

        static string msg = "";
        public static int port = 31416;
        static bool puertoOcupado = true;

        static void funcionHilo(object socket)
        {
            using (Socket sCliente = (Socket)socket)
            {
                IPEndPoint ipCliente = (IPEndPoint)sCliente.RemoteEndPoint;
                Console.WriteLine("Estás conected");

                using (NetworkStream ns = new NetworkStream(sCliente))
                using (StreamReader sr = new StreamReader(ns))
                using (StreamWriter sw = new StreamWriter(ns))

                {
                    msg = sr.ReadLine();



                    try
                    {


                        if (msg != null)
                        {

                            switch (msg)
                            {


                                case "HORA":
                                    Console.WriteLine(msg);
                                    sw.WriteLine(DateTime.Now.ToString("T"));
                                    break;


                                case "FECHA":
                                    sw.WriteLine(DateTime.Now.ToString("D"));

                                    break;



                                case "TODO":
                                    sw.WriteLine(DateTime.Now);

                                    break;


                                case "APAGAR":

                                    break;


                            }
                        }
                    }


                    catch (Exception e) when (e is NullReferenceException || e is IOException)
                    {


                    }


                    Console.WriteLine("Client disconnected.\nConnection closed");

                }

            }












        }
        static void Main(string[] args)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                do
                {
                    try
                    {

                        IPEndPoint ie = new IPEndPoint(IPAddress.Any, port);
                        s.Bind(ie);

                    }
                    catch (SocketException e)
                    {
                        port++;

                    }




                } while (!puertoOcupado);


                s.Listen(10);


                while (msg != "APAGAR")
                {
                    Socket sCliente = s.Accept();
                    Thread hilo = new Thread(funcionHilo);
                    hilo.Start(sCliente);
                }




                Console.WriteLine("Estás conected");



            }
        }
    }
}

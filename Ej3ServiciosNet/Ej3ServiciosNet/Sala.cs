using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ej3ServiciosNet
{
    class Sala
    {



        public static List<Socket> clientes = new List<Socket>();
       






        public static int leerPuerto()
        {

            int puerto;
            int res=0;
            string path = @"C:\temp\puerto.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                try
                {
                    puerto = Convert.ToInt32(sr.ReadLine());
                }
                catch (ArgumentException e)
                {

                    puerto = 10000;
                }
               
            }


            if (puerto<0)
            {
                res = 10000;
            }
            else
            {
                res = puerto;
            }
           
           


                return res;
        }

        public static void envioMensaje(string m, IPEndPoint ie)
        {
            foreach (Socket cliente in clientes)
            {
                using (cliente)
                {
                    using(NetworkStream ns = new NetworkStream(cliente))
                    using(StreamWriter sw=new StreamWriter(ns))
                    {
                        IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
                        if (ieCliente.Port!=ie.Port)
                        {
                            sw.WriteLine("IP:{0} {1}", ie.Address, m);
                            sw.Flush();
                        }
                       
                           
                        
                       
                    }
                }
            }
        }


      public  void iniciaServicioChat()
        {

            bool puertoOcupado = true;
            int puerto = leerPuerto();
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {

                do
                {
                    IPEndPoint ie = new IPEndPoint(IPAddress.Parse("127.0.0.1"), puerto);
                    try
                    {

                        s.Bind(ie);
                        Console.WriteLine("Conectado al server");
                        puertoOcupado = false;



                    }
                    catch (SocketException)
                    {
                        puerto++;
                        if (puerto > 65535)
                        {
                            puerto = 10000;
                        }
                        
                    }





                } while (puertoOcupado);

                Console.WriteLine(puerto);
                s.Listen(10);
                while (true)
                {
                    Socket sCliente = s.Accept();
                    clientes.Add(sCliente);
                    Thread hilo = new Thread(hiloCliente);
                    hilo.Start(sCliente);


                }


            }

        }


        static void hiloCliente(object socket)
        {
            using (Socket cliente = (Socket)socket)
            {
                string userMsg="";
                using (NetworkStream ns = new NetworkStream(cliente))
                using(StreamWriter sw=new StreamWriter(ns))
                using(StreamReader sr=new StreamReader(ns))
                {

                        IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
                        Console.WriteLine("la ip del cliente es: {0}, el puerto es {1}", ieCliente.Address, ieCliente.Port);
                        sw.WriteLine("BIENVENIDO, ahora mismo hay "+clientes.Count +" clientes");
                        sw.Flush();
                   
                    while (userMsg!="MELARGO")
                    {
                        try
                        {
                            userMsg = sr.ReadLine();

                            if (userMsg != null)
                            {
                                envioMensaje(userMsg, ieCliente);

                            }

                        }
                        catch (Exception)
                        {

                            
                        }
                    }
                   






                }
            }

            Console.WriteLine("se cerró conexión");
        }






    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ej4ServiciosNet
{
    class ServidorArchivos
    {

        public int puerto;
        public bool funcionando = true;


        public string leerArchivo(string nombreArchivo, int nLineas)
        {
            string resultado = "";
            string line = "";
            int cont = 0;
            try
            {
                using (StreamReader sr = new StreamReader(Environment.GetEnvironmentVariable("EXAMEN") + "\\" + nombreArchivo))
                {
                    while ((line = sr.ReadLine()) != null && cont < nLineas)
                    {
                        resultado += line;
                        resultado += "\n";
                        cont++;
                    }
                    return resultado;
                }

            }
            catch (IOException)
            {

                return "<ERROR_IO>";
            }

        }



        public int leePuerto()
        {
            try
            {
                puerto = Convert.ToInt32(leerArchivo("puerto.txt", 1));
            }
            catch (ArgumentException)
            {

                return 31416;
            }


            if (puerto < IPEndPoint.MinPort || puerto > IPEndPoint.MaxPort)
            {
                puerto = 31416;
            }

            return puerto;

        }

        public void guardarPuerto(int numero)
        {
            using (StreamWriter sw = new StreamWriter(Environment.GetEnvironmentVariable("EXAMEN") + "\\puerto.txt"))
            {
                sw.WriteLine(numero);
            }
        }


        public string listaArchivos()
        {
            string res = "";
            DirectoryInfo di = new DirectoryInfo(Environment.GetEnvironmentVariable("EXAMEN"));
            foreach (FileInfo f in di.GetFiles())
            {
                if (f.Extension == ".txt")
                {
                    res += f.Name + "\n";
                }
            }

            return res;


        }



        public void iniciaServidorArchivos()
        {

            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint ie = new IPEndPoint(IPAddress.Any, leePuerto());
                try
                {
                    s.Bind(ie);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("puerto ocupado");
                    funcionando = false;
                }

                Console.WriteLine("esperando conexión en el puerto " + ie.Port + " ...");
                s.Listen(10);
                while (funcionando)
                {
                    Socket sCliente = s.Accept();
                    Thread hilo = new Thread(hiloCliente);
                    hilo.Start(sCliente);
                }


            }
        }


        public void hiloCliente(object socket)
        {
            Socket sCliente = (Socket)socket;
            using (sCliente)
            {
                IPEndPoint ieCliente = (IPEndPoint)sCliente.RemoteEndPoint;
                Console.WriteLine("IP: {0} Port: {1}", ieCliente.Address, ieCliente.Port);
                using (NetworkStream ns = new NetworkStream(sCliente))
                using (StreamWriter sw = new StreamWriter(ns))
                using (StreamReader sr = new StreamReader(ns))
                {

                    sw.WriteLine("CONEXION ESTABLECIDA");
                    sw.Flush();
                    string msg = "";
                    while (msg != null && msg != "CLOSE" && funcionando)
                    {
                        try
                        {
                            msg = sr.ReadLine();







                            string[] palabras = msg.Split(' ', ',');

                            switch (palabras[0])
                            {
                                case "GET":
                                    string archivo = palabras[1];
                                    Int32.TryParse(palabras[2], out int n);
                                    Console.WriteLine(archivo);
                                    sw.WriteLine(leerArchivo(archivo, n));
                                    sw.Flush();
                                    break;
                                case "PORT":
                                    int num = Convert.ToInt32(palabras[1]);
                                    guardarPuerto(num);

                                    break;


                                case "LIST":
                                    sw.WriteLine(listaArchivos());
                                    sw.Flush();
                                    break;


                                case "HALT":




                                default:
                                    break;
                            }


                        }

                        
                        catch (IOException e)
                    {

                        break;
                    }
                    catch (NullReferenceException e)
                    {
                        break;
                    }


                }
            }

        }
        Console.WriteLine("Se ha perdido la conexión");
        }
}
}

